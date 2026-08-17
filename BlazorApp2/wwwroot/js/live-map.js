/* ═══════════════════════════════════════════════════════════════
   DERS — Live Map  (wwwroot/js/live-map.js)
   Leaflet map for the /admin/map page.
   Called from LiveMap.razor via JS interop.
   ═══════════════════════════════════════════════════════════════ */
window.DersLiveMap = (() => {

    const SYRIA_CENTER = [34.8, 38.9];
    const SYRIA_BOUNDS = [[32.3, 35.6], [37.3, 42.4]];
    const OSM_TILE = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';

    let _map = null;
    let _disasterLayer = null;   // LayerGroup for disaster markers
    let _teamLayer = null;       // LayerGroup for team markers
    let _lineLayer = null;       // LayerGroup for assignment polylines

    let _disasterMarkers = {};   // { disasterId(string): L.Marker }
    let _teamMarkers = {};       // { teamId(string): { marker, disasterId, line } }
    let _disasterColors = {};    // { disasterId(string): colorHex }

    /* ── Colour palette for disaster grouping ──────────────────────── */
    const DISASTER_PALETTE = [
        '#ef4444', '#f97316', '#eab308', '#22c55e',
        '#06b6d4', '#8b5cf6', '#ec4899', '#14b8a6',
        '#f43f5e', '#a855f7',
    ];
    let _paletteIndex = 0;
    let _disasterData = {};      // { disasterId(string): last full pin payload }

    function _assignColor(disasterId) {
        if (!_disasterColors[disasterId]) {
            _disasterColors[disasterId] = DISASTER_PALETTE[_paletteIndex % DISASTER_PALETTE.length];
            _paletteIndex++;
        }
        return _disasterColors[disasterId];
    }

    /* ── Init ──────────────────────────────────────────────────────── */
    function init(elementId) {
        if (_map) { try { _map.remove(); } catch { } _map = null; }

        // Reset state
        _disasterMarkers = {}; _teamMarkers = {};
        _disasterColors = {}; _paletteIndex = 0;
        _disasterData = {};

        const el = document.getElementById(elementId);
        if (!el) { console.warn('[LiveMap] element not found:', elementId); return; }

        _map = L.map(elementId, {
            center: SYRIA_CENTER, zoom: 7,
            minZoom: 5, maxZoom: 17,
            maxBounds: SYRIA_BOUNDS, maxBoundsViscosity: 0.9,
            zoomControl: false,
        });

        L.control.zoom({ position: 'bottomright' }).addTo(_map);

        L.tileLayer(OSM_TILE, {
            attribution: '© OpenStreetMap contributors',
            maxZoom: 19
        }).addTo(_map);

        // Layer order: lines → disasters → teams
        _lineLayer = L.layerGroup().addTo(_map);
        _disasterLayer = L.layerGroup().addTo(_map);
        _teamLayer = L.layerGroup().addTo(_map);

        setTimeout(() => _map?.invalidateSize(), 150);
        console.log('[LiveMap] Initialised ✅');
    }

    /* ── Disaster pin ──────────────────────────────────────────────── */
    function addPin(d) {
        if (!_map) return;
        if (d.lat == null || d.lng == null) return;

        const lat = parseFloat(d.lat);
        const lng = parseFloat(d.lng);
        if (isNaN(lat) || isNaN(lng)) return;

        const id = String(d.id);
        _disasterData[id] = d; // remember it for later status updates

        const color = _assignColor(id);
        const isResolved = (d.status || '').toLowerCase() === 'resolved'
            || (d.status || '').toLowerCase() === 'closed';
        const pinColor = isResolved ? '#6b7280' : color;
        const icon = _disasterIcon(d.type, pinColor, isResolved);

        const marker = L.marker([lat, lng], { icon });

        marker.bindPopup(_disasterPopup(d, pinColor), { maxWidth: 300, className: 'ders-popup' });

        _disasterMarkers[id] = marker;
        _disasterLayer.addLayer(marker);
        // Draw tethers for any team pins that arrived before this disaster pin did
        Object.keys(_teamMarkers).forEach(teamId => {
            const entry = _teamMarkers[teamId];
            if (entry.disasterId === id && !entry.line) {
                const pos = entry.marker.getLatLng();
                _refreshTether(teamId, pos.lat, pos.lng, id);
            }
        });
    }

    /* ── Team pin ──────────────────────────────────────────────────── */
    /**
     * @param {object} t - team data
     * @param {string|null} t.teamId      - team GUID
     * @param {number}      t.lat
     * @param {number}      t.lng
     * @param {string|null} t.name        - team name
     * @param {string|null} t.speciality
     * @param {string|null} t.status
     * @param {string|null} t.disasterId  - assigned disaster GUID (null if unassigned)
     * @param {string|null} t.leaderName
     * @param {number}      t.membersCount
     */
    function updateTeamPin(t) {
        if (!_map) return;

        // Support old-style (teamId, lat, lng) call signature for backward compat
        if (typeof t !== 'object') {
            const [teamId, lat, lng] = arguments;
            t = { teamId: String(teamId), lat, lng };
        }

        const lat = parseFloat(t.lat);
        const lng = parseFloat(t.lng);
        if (isNaN(lat) || isNaN(lng)) return;

        const id = String(t.teamId);
        const disasterId = t.disasterId ? String(t.disasterId) : null;
        const accentColor = disasterId ? _assignColor(disasterId) : '#3b82f6';

        if (_teamMarkers[id]) {
            const entry = _teamMarkers[id];

            // Update position
            entry.marker.setLatLng([lat, lng]);

            // Refresh icon (status) and popup content — without this, status
            // changes (e.g. Active -> Returning) never show up on an existing pin
            entry.marker.setIcon(_teamIcon(accentColor, t.status));
            entry.marker.setPopupContent(_teamPopup(t, accentColor));

            // Update tether line
            _refreshTether(id, lat, lng, disasterId);
        } else {
            // Create marker
            const marker = L.marker([lat, lng], {
                icon: _teamIcon(accentColor, t.status),
                zIndexOffset: 200,
            });

            marker.bindPopup(_teamPopup(t, accentColor), { maxWidth: 260, className: 'ders-popup' });

            _teamMarkers[id] = { marker, disasterId, line: null };
            _teamLayer.addLayer(marker);
            _refreshTether(id, lat, lng, disasterId);
        }
    }

    /* ── Disaster status update (in place, no marker re-creation) ───── */
    function updatePinStatus(id, status) {
        if (!_map) return;
        const key = String(id);
        const marker = _disasterMarkers[key];
        if (!marker) return;

        // Merge the new status into whatever we last knew about this pin
        const d = { ..._disasterData[key], status };
        _disasterData[key] = d;

        const color = _assignColor(key);
        const isResolved = (status || '').toLowerCase() === 'resolved'
            || (status || '').toLowerCase() === 'closed';
        const pinColor = isResolved ? '#6b7280' : color;

        marker.setIcon(_disasterIcon(d.type, pinColor, isResolved));
        marker.setPopupContent(_disasterPopup(d, pinColor));
    }

    /* ── Remove a disaster pin entirely (e.g. on close) ──────────────── */
    function removePin(id) {
        if (!_map) return;
        const key = String(id);

        const marker = _disasterMarkers[key];
        if (marker) {
            _disasterLayer.removeLayer(marker);
            delete _disasterMarkers[key];
        }
        delete _disasterData[key];

        // Drop tether lines still pointing at this disaster
        Object.keys(_teamMarkers).forEach(teamId => {
            const entry = _teamMarkers[teamId];
            if (entry.disasterId === key) {
                if (entry.line) {
                    _lineLayer.removeLayer(entry.line);
                    entry.line = null;
                }
                entry.disasterId = null;
            }
        });
    }

    /* ── Tether line (team → disaster) ────────────────────────────── */
    function _refreshTether(teamId, lat, lng, disasterId) {
        const entry = _teamMarkers[teamId];
        if (!entry) return;

        if (entry.line) {
            _lineLayer.removeLayer(entry.line);
            entry.line = null;
        }

        entry.disasterId = disasterId;   // ← moved here

        if (!disasterId) return;
        const dm = _disasterMarkers[disasterId];
        if (!dm) return;

        const dLatLng = dm.getLatLng();
        const color = _assignColor(disasterId);

        const line = L.polyline([[lat, lng], [dLatLng.lat, dLatLng.lng]], {
            color, weight: 2, opacity: 0.55, dashArray: '6 5', className: 'ders-tether',
        });

        entry.line = line;
        _lineLayer.addLayer(line);
        // note: entry.disasterId = disasterId; line removed from here, it's set above now
    }

    /* ── flyTo / centre / destroy ──────────────────────────────────── */
    function flyTo(id, lat, lng) {
        if (!_map) return;
        _map.flyTo([parseFloat(lat), parseFloat(lng)], 13, { duration: 1.4 });
        setTimeout(() => {
            const m = _disasterMarkers[String(id)];
            if (m) m.openPopup();
        }, 1500);
    }

    function centre() {
        _map?.flyTo(SYRIA_CENTER, 7, { duration: 1.2 });
    }

    function destroy() {
        if (_map) { try { _map.remove(); } catch { } _map = null; }
        _disasterLayer = null; _teamLayer = null; _lineLayer = null;
        _disasterMarkers = {}; _teamMarkers = {};
        _disasterColors = {}; _paletteIndex = 0;
        _disasterData = {};
    }

    /* ── Icon builders ─────────────────────────────────────────────── */
    function _disasterIcon(type, color, resolved) {
        const fa = _disasterFaIcon(type);
        const glow = resolved ? '' : `box-shadow:0 0 0 4px ${color}33,0 2px 10px rgba(0,0,0,.4)`;
        const opacity = resolved ? '0.55' : '1';
        return L.divIcon({
            html: `<div style="
                width:38px;height:38px;border-radius:50%;
                background:${color};border:2.5px solid #fff;
                display:flex;align-items:center;justify-content:center;
                font-size:15px;color:#fff;opacity:${opacity};
                ${glow};transition:transform .15s ease">
                <i class='fa-solid ${fa}'></i>
            </div>`,
            className: 'ders-disaster-pin',
            iconSize: [38, 38], iconAnchor: [19, 19], popupAnchor: [0, -22],
        });
    }

    function _teamIcon(accentColor, status) {
        const isActive = (status || '').toLowerCase() === 'active'
            || (status || '').toLowerCase() === 'deployed';
        const pulse = isActive
            ? `animation:ders-pulse 1.8s ease infinite`
            : '';
        return L.divIcon({
            html: `<div style="
                width:30px;height:30px;border-radius:50%;
                background:#0f172a;border:3px solid ${accentColor};
                display:flex;align-items:center;justify-content:center;
                font-size:12px;color:${accentColor};
                box-shadow:0 0 0 2px ${accentColor}44,0 2px 8px rgba(0,0,0,.5);
                ${pulse}">
                <i class='fa-solid fa-people-group'></i>
            </div>`,
            className: 'ders-team-pin',
            iconSize: [30, 30], iconAnchor: [15, 15], popupAnchor: [0, -18],
        });
    }

    /* ── Popup builders ────────────────────────────────────────────── */
    function _disasterPopup(d, color) {
        const loc = [d.province, d.city].filter(Boolean).join(', ');
        const statusLabel = _esc(d.status || 'Unknown');
        const statusBg = _statusBg(d.status);
        return `
        <div style="min-width:220px;font-family:'DM Sans',sans-serif">
            <div style="display:flex;align-items:center;gap:8px;margin-bottom:10px">
                <div style="width:10px;height:10px;border-radius:50%;background:${color};flex-shrink:0;
                            box-shadow:0 0 0 3px ${color}33"></div>
                <span style="font-size:14px;font-weight:700;color:${color};line-height:1.3">
                    ${_esc(d.title)}
                </span>
            </div>
            ${loc ? `<div style="font-size:12px;color:#8a92a8;margin-bottom:4px">
                <i class='fa-solid fa-location-dot' style="margin-right:4px"></i>${_esc(loc)}
            </div>` : ''}
            <div style="display:flex;align-items:center;gap:8px;margin:8px 0">
                <span style="font-size:11px;font-weight:700;padding:2px 8px;border-radius:20px;
                             background:${statusBg.bg};color:${statusBg.fg};text-transform:uppercase">
                    ${statusLabel}
                </span>
                ${d.type ? `<span style="font-size:11px;color:#8a92a8">${_esc(d.type)}</span>` : ''}
            </div>
            ${d.teamsCount ? `<div style="font-size:12px;color:#8a92a8;margin-bottom:8px">
                <i class='fa-solid fa-people-group' style="margin-right:4px;color:${color}"></i>
                ${d.teamsCount} team${d.teamsCount !== 1 ? 's' : ''} assigned
            </div>` : ''}
            <a href="${_esc(d.detailUrl || '#')}"
               style="display:inline-flex;align-items:center;gap:6px;
                      padding:5px 14px;border-radius:8px;
                      background:${color};color:#fff;font-size:12px;
                      font-weight:600;text-decoration:none;margin-top:4px">
                <i class='fa-solid fa-eye'></i> View Details
            </a>
        </div>`;
    }

    function _teamPopup(t, accentColor) {
        const disasterColor = t.disasterId ? _assignColor(String(t.disasterId)) : accentColor;
        return `
        <div style="min-width:200px;font-family:'DM Sans',sans-serif">
            <div style="display:flex;align-items:center;gap:8px;margin-bottom:10px">
                <div style="width:28px;height:28px;border-radius:50%;
                            background:#0f172a;border:2px solid ${disasterColor};
                            display:flex;align-items:center;justify-content:center;
                            font-size:11px;color:${disasterColor}">
                    <i class='fa-solid fa-people-group'></i>
                </div>
                <div>
                    <div style="font-size:13px;font-weight:700;color:#f1f5f9">
                        ${_esc(t.name || 'Team')}
                    </div>
                    ${t.speciality ? `<div style="font-size:11px;color:#8a92a8">${_esc(t.speciality)}</div>` : ''}
                </div>
            </div>
            ${t.leaderName ? `<div style="font-size:12px;color:#8a92a8;margin-bottom:4px">
                <i class='fa-solid fa-user-tie' style="margin-right:4px"></i>${_esc(t.leaderName)}
            </div>` : ''}
            ${t.membersCount ? `<div style="font-size:12px;color:#8a92a8;margin-bottom:4px">
                <i class='fa-solid fa-users' style="margin-right:4px"></i>
                ${t.membersCount} member${t.membersCount !== 1 ? 's' : ''}
                ${t.activeMembersCount ? `<span style="color:${disasterColor};margin-left:4px">(${t.activeMembersCount} active)</span>` : ''}
            </div>` : ''}
            <div style="margin-top:6px">
                <span style="font-size:11px;font-weight:700;padding:2px 8px;border-radius:20px;
                             background:${disasterColor}22;color:${disasterColor};text-transform:uppercase">
                    ${_esc(t.status || 'Unknown')}
                </span>
            </div>
            ${t.disasterId ? `<div style="margin-top:8px;padding-top:8px;border-top:1px solid #1e293b;
                                         font-size:11px;color:#8a92a8">
                <span style="display:inline-block;width:8px;height:8px;border-radius:50%;
                             background:${disasterColor};margin-right:4px"></span>
                Assigned to incident
            </div>` : ''}
        </div>`;
    }

    /* ── Helpers ───────────────────────────────────────────────────── */
    function _disasterFaIcon(type) {
        const m = {
            Earthquake: 'fa-house-crack', Flood: 'fa-water', Fire: 'fa-fire',
            Chemical: 'fa-biohazard', Explosion: 'fa-explosion',
            Hurricane: 'fa-tornado', Landslide: 'fa-hill-rockslide',
        };
        return m[type] || 'fa-circle-exclamation';
    }

    function _statusBg(status) {
        const s = (status || '').toLowerCase();
        if (s === 'resolved' || s === 'closed') return { bg: '#1e293b', fg: '#64748b' };
        if (s === 'inprogress' || s === 'in progress') return { bg: '#1e3a5f', fg: '#60a5fa' };
        return { bg: '#450a0a', fg: '#f87171' }; // active/open
    }

    function _esc(s) {
        return String(s ?? '').replace(/[&<>"']/g, c =>
            ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    }

    /* ── Inject keyframes once ─────────────────────────────────────── */
    (function _injectStyles() {
        if (document.getElementById('ders-map-styles')) return;
        const s = document.createElement('style');
        s.id = 'ders-map-styles';
        s.textContent = `
            @keyframes ders-pulse {
                0%,100% { box-shadow: 0 0 0 2px var(--ac,#3b82f6)44, 0 2px 8px rgba(0,0,0,.5); }
                50%      { box-shadow: 0 0 0 7px transparent, 0 2px 8px rgba(0,0,0,.5); }
            }
            .ders-disaster-pin, .ders-team-pin { background: transparent !important; border: none !important; }
            .ders-popup .leaflet-popup-content-wrapper {
                background: #0f172a;
                border: 1px solid #1e293b;
                border-radius: 12px;
                box-shadow: 0 8px 32px rgba(0,0,0,.6);
                color: #f1f5f9;
            }
            .ders-popup .leaflet-popup-tip { background: #0f172a; }
            .ders-popup .leaflet-popup-close-button { color: #64748b !important; }
            .ders-tether { pointer-events: none; }
        `;
        document.head.appendChild(s);
    })();

    /* ── Public API ────────────────────────────────────────────────── */
    return { init, addPin, updateTeamPin, updatePinStatus, removePin, flyTo, centre, destroy };
})();