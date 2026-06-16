/* ═══════════════════════════════════════════════════════════════
   DERS — Detail Map  (wwwroot/js/detail-map.js)
   Single-pin Leaflet map for the DisasterDetail page.
   Matches live-map.js tile, pin style, popup and dark theme exactly.
   Called from DisasterDetail.razor via JS interop.
   ═══════════════════════════════════════════════════════════════ */
window.DersDetailMap = (() => {

    const OSM_TILE = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
    const SYRIA_BOUNDS = [[32.3, 35.6], [37.3, 42.4]];

    /* Same palette as live-map.js — index 0 used for single pin */
    const DISASTER_PALETTE = [
        '#ef4444', '#f97316', '#eab308', '#22c55e',
        '#06b6d4', '#8b5cf6', '#ec4899', '#14b8a6',
        '#f43f5e', '#a855f7',
    ];

    let _map = null;
    let _marker = null;

    /* ── Inject shared styles (identical to live-map.js) ──────────── */
    function _injectStyles() {
        if (document.getElementById('ders-map-styles')) return;
        const s = document.createElement('style');
        s.id = 'ders-map-styles';
        s.textContent = `
            @keyframes ders-pulse {
                0%,100% { box-shadow: 0 0 0 2px var(--ac,#3b82f6)44, 0 2px 8px rgba(0,0,0,.5); }
                50%      { box-shadow: 0 0 0 7px transparent,          0 2px 8px rgba(0,0,0,.5); }
            }
            .ders-disaster-pin, .ders-team-pin {
                background: transparent !important;
                border: none !important;
            }
            .ders-popup .leaflet-popup-content-wrapper {
                background: #0f172a;
                border: 1px solid #1e293b;
                border-radius: 12px;
                box-shadow: 0 8px 32px rgba(0,0,0,.6);
                color: #f1f5f9;
            }
            .ders-popup .leaflet-popup-tip   { background: #0f172a; }
            .ders-popup .leaflet-popup-close-button { color: #64748b !important; }
        `;
        document.head.appendChild(s);
    }

    /* ── Icon — identical to live-map.js _disasterIcon() ──────────── */
    function _buildIcon(type, color, resolved) {
        const fa = _faIcon(type);
        const opacity = resolved ? '0.55' : '1';
        const glow = resolved ? '' : `box-shadow:0 0 0 4px ${color}33,0 2px 10px rgba(0,0,0,.4)`;
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
            iconSize: [38, 38],
            iconAnchor: [19, 19],
            popupAnchor: [0, -22],
        });
    }

    /* ── Popup — identical markup/colours to live-map.js ──────────── */
    function _buildPopup(title, type, status, province, city, color) {
        const loc = [province, city].filter(Boolean).join(', ');
        const statusBg = _statusBg(status);
        const statusLabel = _esc(status || 'Unknown');
        return `
        <div style="min-width:220px;font-family:'DM Sans',sans-serif">
            <div style="display:flex;align-items:center;gap:8px;margin-bottom:10px">
                <div style="width:10px;height:10px;border-radius:50%;background:${color};flex-shrink:0;
                            box-shadow:0 0 0 3px ${color}33"></div>
                <span style="font-size:14px;font-weight:700;color:${color};line-height:1.3">
                    ${_esc(title)}
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
                ${type ? `<span style="font-size:11px;color:#8a92a8">${_esc(type)}</span>` : ''}
            </div>
        </div>`;
    }

    /* ── Init ──────────────────────────────────────────────────────── */
    function init(elementId, lat, lng, title, status, type, province, city) {
        _injectStyles();

        // Destroy previous instance if navigating back to the tab
        if (_map) { try { _map.remove(); } catch { } _map = null; _marker = null; }

        const el = document.getElementById(elementId);
        if (!el) { console.warn('[DetailMap] element not found:', elementId); return; }

        const resolved = _isResolved(status);
        const color = resolved ? '#6b7280' : DISASTER_PALETTE[0];

        _map = L.map(elementId, {
            center: [lat, lng],
            zoom: 13,
            minZoom: 5,
            maxZoom: 17,
            maxBounds: SYRIA_BOUNDS,
            maxBoundsViscosity: 0.9,
            zoomControl: false,
        });

        L.control.zoom({ position: 'bottomright' }).addTo(_map);

        /* Same OSM tile as live-map.js */
        L.tileLayer(OSM_TILE, {
            attribution: '© OpenStreetMap contributors',
            maxZoom: 19,
        }).addTo(_map);

        /* Pin */
        const icon = _buildIcon(type, color, resolved);
        _marker = L.marker([lat, lng], { icon })
            .addTo(_map)
            .bindPopup(
                _buildPopup(title, type, status, province, city, color),
                { maxWidth: 300, className: 'ders-popup' }
            )
            .openPopup();

        setTimeout(() => _map?.invalidateSize(), 150);
        console.log('[DetailMap] Initialised ✅');
    }

    /* ── Destroy (called on tab leave / page dispose) ──────────────── */
    function destroy() {
        if (_map) { try { _map.remove(); } catch { } _map = null; }
        _marker = null;
    }

    /* ── Helpers (mirrors live-map.js) ────────────────────────────── */
    function _faIcon(type) {
        const m = {
            Earthquake: 'fa-house-crack', Flood: 'fa-water', Fire: 'fa-fire',
            Chemical: 'fa-biohazard', Explosion: 'fa-explosion',
            Hurricane: 'fa-tornado', Landslide: 'fa-hill-rockslide',
        };
        return m[type] || 'fa-circle-exclamation';
    }

    function _statusBg(status) {
        const s = (status || '').toLowerCase();
        if (s === 'resolved' || s === 'closed')
            return { bg: '#1e293b', fg: '#64748b' };
        if (s === 'inprogress' || s === 'in progress')
            return { bg: '#1e3a5f', fg: '#60a5fa' };
        return { bg: '#450a0a', fg: '#f87171' }; // reported / open
    }

    function _isResolved(status) {
        const s = (status || '').toLowerCase();
        return s === 'resolved' || s === 'closed';
    }

    function _esc(s) {
        return String(s ?? '').replace(/[&<>"']/g, c =>
            ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    }

    /* ── Public API ────────────────────────────────────────────────── */
    return { init, destroy };

})();