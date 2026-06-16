window.mapInterop = {
    _maps: {},

    // ── Generic map (reusable with dotNetRef callbacks) ──────────────
    initMap: function (mapId, lat, lng, dotNetRef) {
        if (this._maps[mapId]) {
            this._maps[mapId].remove();
            delete this._maps[mapId];
        }

        var el = document.getElementById(mapId);
        if (!el) { console.error('[mapInterop] Element not found:', mapId); return; }

        var map = L.map(mapId).setView([lat, lng], 7);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);

        var marker = L.marker([lat, lng], { draggable: true }).addTo(map);

        map.on('click', function (e) {
            marker.setLatLng(e.latlng);
            dotNetRef.invokeMethodAsync('OnLocationSelected', e.latlng.lat, e.latlng.lng);
        });
        marker.on('dragend', function () {
            var pos = marker.getLatLng();
            dotNetRef.invokeMethodAsync('OnLocationSelected', pos.lat, pos.lng);
        });

        this._maps[mapId] = map;
        setTimeout(function () { map.invalidateSize(); }, 400);
    },

    // ── Mini map for Create Team modal ──────────────────────────────
    // Uses OSM tiles (same as detail-map.js) for full geographic detail.
    // Custom team pin + styled popup + drag support + click-hint overlay.
    initCreateMap: function () {
        var el = document.getElementById('createTeamMap');
        if (!el) { console.error('[mapInterop] createTeamMap div not found'); return; }

        if (this._maps['createTeamMap']) {
            this._maps['createTeamMap'].remove();
            delete this._maps['createTeamMap'];
        }

        // ── Inject shared pin/popup styles (mirrors detail-map.js) ───
        if (!document.getElementById('ders-map-styles')) {
            var s = document.createElement('style');
            s.id = 'ders-map-styles';
            s.textContent = [
                '@keyframes ders-pulse{',
                '0%,100%{box-shadow:0 0 0 2px rgba(59,130,246,.27),0 2px 8px rgba(0,0,0,.5)}',
                '50%{box-shadow:0 0 0 8px transparent,0 2px 8px rgba(0,0,0,.5)}}',
                '.ders-disaster-pin,.ders-team-pin{background:transparent!important;border:none!important}',
                '.ders-popup .leaflet-popup-content-wrapper{',
                '  background:#0f172a;border:1px solid #1e293b;border-radius:12px;',
                '  box-shadow:0 8px 32px rgba(0,0,0,.6);color:#f1f5f9}',
                '.ders-popup .leaflet-popup-tip{background:#0f172a}',
                '.ders-popup .leaflet-popup-close-button{color:#64748b!important}',
            ].join('');
            document.head.appendChild(s);
        }

        // ── OSM tiles — same source as detail-map.js for full detail ─
        var SYRIA_BOUNDS = [[32.3, 35.6], [37.3, 42.4]];
        var TEAM_COLOR = '#3b82f6'; // blue — matches team pin in live-map.js

        var map = L.map('createTeamMap', {
            center: [34.8021, 38.9968],
            zoom: 7,
            minZoom: 5,
            maxZoom: 17,
            maxBounds: SYRIA_BOUNDS,
            maxBoundsViscosity: 0.9,
            zoomControl: false,
        });

        L.control.zoom({ position: 'bottomright' }).addTo(map);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
            maxZoom: 19,
        }).addTo(map);

        // ── Click-hint overlay (disappears after first pin drop) ──────
        var hint = document.createElement('div');
        hint.id = 'createMapHint';
        hint.style.cssText = [
            'position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);',
            'background:rgba(15,23,42,.82);color:#f1f5f9;',
            'padding:10px 18px;border-radius:10px;border:1px solid #1e293b;',
            'font-size:13px;font-family:\'DM Sans\',sans-serif;',
            'pointer-events:none;z-index:1000;',
            'display:flex;align-items:center;gap:8px;',
        ].join('');
        hint.innerHTML = '<i class="fa-solid fa-hand-pointer" style="color:#3b82f6"></i> Click the map to place the team\'s location';
        el.style.position = 'relative';
        el.appendChild(hint);

        // ── Custom team pin icon (mirrors live-map.js team pin style) ─
        function makePinIcon() {
            return L.divIcon({
                html: '<div style="' +
                    'width:38px;height:38px;border-radius:50%;' +
                    'background:#0f172a;border:3px solid ' + TEAM_COLOR + ';' +
                    'display:flex;align-items:center;justify-content:center;' +
                    'font-size:15px;color:' + TEAM_COLOR + ';' +
                    'animation:ders-pulse 1.8s ease infinite;' +
                    'box-shadow:0 0 0 2px rgba(59,130,246,.27),0 2px 8px rgba(0,0,0,.5)">' +
                    '<i class="fa-solid fa-people-group"></i>' +
                    '</div>',
                className: 'ders-team-pin',
                iconSize: [38, 38],
                iconAnchor: [19, 19],
                popupAnchor: [0, -24],
            });
        }

        // ── Styled popup (mirrors detail-map.js _buildPopup) ─────────
        function makePopup(lat, lng) {
            return '<div style="min-width:200px;font-family:\'DM Sans\',sans-serif">' +
                '<div style="display:flex;align-items:center;gap:8px;margin-bottom:8px">' +
                '<div style="width:10px;height:10px;border-radius:50%;background:' + TEAM_COLOR + ';' +
                'box-shadow:0 0 0 3px ' + TEAM_COLOR + '33;flex-shrink:0"></div>' +
                '<span style="font-size:14px;font-weight:700;color:' + TEAM_COLOR + '">Team Location</span>' +
                '</div>' +
                '<div style="font-size:12px;color:#8a92a8;margin-bottom:6px">' +
                '<i class="fa-solid fa-location-dot" style="margin-right:4px"></i>' +
                parseFloat(lat).toFixed(5) + ',  ' + parseFloat(lng).toFixed(5) +
                '</div>' +
                '<div style="font-size:11px;color:#475569">' +
                '<i class="fa-solid fa-arrows-up-down-left-right" style="margin-right:4px"></i>' +
                'Drag pin or click map to adjust' +
                '</div>' +
                '</div>';
        }

        var marker = null;
        var latEl = document.getElementById('teamLat');
        var lngEl = document.getElementById('teamLng');

        function placePin(lat, lng) {
            if (latEl) latEl.value = lat.toFixed(6);
            if (lngEl) lngEl.value = lng.toFixed(6);

            // Hide the hint overlay after first placement
            var h = document.getElementById('createMapHint');
            if (h) h.style.display = 'none';

            if (marker) {
                marker.setLatLng([lat, lng]);
                marker.setPopupContent(makePopup(lat, lng));
            } else {
                marker = L.marker([lat, lng], { icon: makePinIcon(), draggable: true })
                    .addTo(map)
                    .bindPopup(makePopup(lat, lng), { maxWidth: 260, className: 'ders-popup' })
                    .openPopup();

                marker.on('dragend', function () {
                    var pos = marker.getLatLng();
                    placePin(pos.lat, pos.lng);
                });
            }
        }

        map.on('click', function (e) { placePin(e.latlng.lat, e.latlng.lng); });

        this._maps['createTeamMap'] = map;
        setTimeout(function () { map.invalidateSize(); }, 400);
        console.log('[mapInterop] createTeamMap initialised ✅');
    },

    // ── Mini map for Report Disaster page ───────────────────────────
    // Uses OSM tiles (same as detail-map.js) for full geographic detail.
    initDisasterMap: function () {
        var el = document.getElementById('reportDisasterMap');
        if (!el) { console.error('[mapInterop] reportDisasterMap div not found'); return; }

        if (this._maps['reportDisasterMap']) {
            this._maps['reportDisasterMap'].remove();
            delete this._maps['reportDisasterMap'];
        }

        // ── Shared styles — same block as detail-map.js ───────────────
        if (!document.getElementById('ders-map-styles')) {
            var s = document.createElement('style');
            s.id = 'ders-map-styles';
            s.textContent = [
                '@keyframes ders-pulse{',
                '0%,100%{box-shadow:0 0 0 2px rgba(239,68,68,.27),0 2px 8px rgba(0,0,0,.5)}',
                '50%{box-shadow:0 0 0 7px transparent,0 2px 8px rgba(0,0,0,.5)}}',
                '.ders-disaster-pin{background:transparent!important;border:none!important}',
                '.ders-popup .leaflet-popup-content-wrapper{',
                '  background:#0f172a;border:1px solid #1e293b;border-radius:12px;',
                '  box-shadow:0 8px 32px rgba(0,0,0,.6);color:#f1f5f9}',
                '.ders-popup .leaflet-popup-tip{background:#0f172a}',
                '.ders-popup .leaflet-popup-close-button{color:#64748b!important}',
            ].join('');
            document.head.appendChild(s);
        }

        var SYRIA_BOUNDS = [[32.3, 35.6], [37.3, 42.4]];

        var map = L.map('reportDisasterMap', {
            center: [34.8021, 38.9968],
            zoom: 7,
            minZoom: 5,
            maxZoom: 17,
            maxBounds: SYRIA_BOUNDS,
            maxBoundsViscosity: 0.9,
            zoomControl: false,
        });

        L.control.zoom({ position: 'bottomright' }).addTo(map);

        // ── OSM tiles — same source as detail-map.js for full detail ──
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
            maxZoom: 19,
        }).addTo(map);

        // ── Click-hint overlay (disappears after first pin drop) ──────
        var hint = document.createElement('div');
        hint.id = 'disasterMapHint';
        hint.style.cssText = [
            'position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);',
            'background:rgba(15,23,42,.82);color:#f1f5f9;',
            'padding:10px 18px;border-radius:10px;border:1px solid #1e293b;',
            'font-size:13px;font-family:\'DM Sans\',sans-serif;',
            'pointer-events:none;z-index:1000;',
            'display:flex;align-items:center;gap:8px;',
        ].join('');
        hint.innerHTML = '<i class="fa-solid fa-hand-pointer" style="color:#ef4444"></i> Click the map to pin the disaster location';
        el.style.position = 'relative';
        el.appendChild(hint);

        // ── Pulsing pin — mirrors detail-map.js _buildIcon() ─────────
        var PIN_COLOR = '#ef4444';  // red — same as DISASTER_PALETTE[0]

        function makePinIcon() {
            return L.divIcon({
                html: '<div style="' +
                    'width:38px;height:38px;border-radius:50%;' +
                    'background:' + PIN_COLOR + ';border:2.5px solid #fff;' +
                    'display:flex;align-items:center;justify-content:center;' +
                    'font-size:15px;color:#fff;' +
                    'animation:ders-pulse 1.8s ease infinite">' +
                    '<i class="fa-solid fa-location-dot"></i>' +
                    '</div>',
                className: 'ders-disaster-pin',
                iconSize: [38, 38],
                iconAnchor: [19, 19],
                popupAnchor: [0, -24],
            });
        }

        // ── Popup — mirrors detail-map.js _buildPopup() ──────────────
        function makePopup(lat, lng) {
            return '<div style="min-width:200px;font-family:\'DM Sans\',sans-serif">' +
                '<div style="display:flex;align-items:center;gap:8px;margin-bottom:8px">' +
                '<div style="width:10px;height:10px;border-radius:50%;background:' + PIN_COLOR + ';flex-shrink:0;' +
                'box-shadow:0 0 0 3px ' + PIN_COLOR + '33"></div>' +
                '<span style="font-size:14px;font-weight:700;color:' + PIN_COLOR + '">Disaster Location</span>' +
                '</div>' +
                '<div style="font-size:12px;color:#8a92a8;margin-bottom:4px">' +
                '<i class="fa-solid fa-location-dot" style="margin-right:4px"></i>' +
                parseFloat(lat).toFixed(5) + ',  ' + parseFloat(lng).toFixed(5) +
                '</div>' +
                '<div style="font-size:11px;color:#475569;margin-top:6px">' +
                '<i class="fa-solid fa-hand-pointer" style="margin-right:4px"></i>' +
                'Click map or drag pin to adjust</div>' +
                '</div>';
        }

        var marker = null;
        var latEl = document.getElementById('disasterLat');
        var lngEl = document.getElementById('disasterLng');

        function placePin(lat, lng) {
            if (latEl) latEl.value = lat.toFixed(6);
            if (lngEl) lngEl.value = lng.toFixed(6);

            // Hide the hint overlay after first placement
            var h = document.getElementById('disasterMapHint');
            if (h) h.style.display = 'none';

            if (marker) {
                marker.setLatLng([lat, lng]);
                marker.setPopupContent(makePopup(lat, lng));
            } else {
                marker = L.marker([lat, lng], { icon: makePinIcon(), draggable: true })
                    .addTo(map)
                    .bindPopup(makePopup(lat, lng), { maxWidth: 280, className: 'ders-popup' })
                    .openPopup();

                marker.on('dragend', function () {
                    var pos = marker.getLatLng();
                    placePin(pos.lat, pos.lng);
                });
            }
        }

        map.on('click', function (e) { placePin(e.latlng.lat, e.latlng.lng); });

        this._maps['reportDisasterMap'] = map;
        setTimeout(function () { map.invalidateSize(); }, 400);
        console.log('[mapInterop] reportDisasterMap initialised ✅');
    },



    // ── Mini map for Register Volunteer step 3 ──────────────────────
    // Uses OSM tiles for full geographic detail, styled pin + popup + hint.
    initVolunteerMap: function () {
        var el = document.getElementById('volunteerMap');
        if (!el) { console.error('[mapInterop] volunteerMap div not found'); return; }

        if (this._maps['volunteerMap']) {
            this._maps['volunteerMap'].remove();
            delete this._maps['volunteerMap'];
        }

        var SYRIA_BOUNDS = [[32.3, 35.6], [37.3, 42.4]];
        var PIN_COLOR = '#10b981'; // green — volunteer theme

        var map = L.map('volunteerMap', {
            center: [34.8021, 38.9968],
            zoom: 7,
            minZoom: 5,
            maxZoom: 17,
            maxBounds: SYRIA_BOUNDS,
            maxBoundsViscosity: 0.9,
            zoomControl: false,
        });

        L.control.zoom({ position: 'bottomright' }).addTo(map);

        // ── OSM tiles — same source as detail-map.js for full detail ──
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
            maxZoom: 19,
        }).addTo(map);

        // ── Click-hint overlay ────────────────────────────────────────
        var hint = document.createElement('div');
        hint.id = 'volunteerMapHint';
        hint.style.cssText = [
            'position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);',
            'background:rgba(15,23,42,.82);color:#f1f5f9;',
            'padding:10px 18px;border-radius:10px;border:1px solid #1e293b;',
            'font-size:13px;font-family:\'DM Sans\',sans-serif;',
            'pointer-events:none;z-index:1000;',
            'display:flex;align-items:center;gap:8px;',
        ].join('');
        hint.innerHTML = '<i class="fa-solid fa-hand-pointer" style="color:#10b981"></i> Click the map to set your location';
        el.style.position = 'relative';
        el.appendChild(hint);

        // ── Custom pin icon ───────────────────────────────────────────
        function makePinIcon() {
            return L.divIcon({
                html: '<div style="' +
                    'width:38px;height:38px;border-radius:50%;' +
                    'background:#0f172a;border:3px solid ' + PIN_COLOR + ';' +
                    'display:flex;align-items:center;justify-content:center;' +
                    'font-size:15px;color:' + PIN_COLOR + ';' +
                    'animation:ders-pulse 1.8s ease infinite;' +
                    'box-shadow:0 0 0 2px rgba(16,185,129,.27),0 2px 8px rgba(0,0,0,.5)">' +
                    '<i class="fa-solid fa-user"></i>' +
                    '</div>',
                className: 'ders-team-pin',
                iconSize: [38, 38],
                iconAnchor: [19, 19],
                popupAnchor: [0, -24],
            });
        }

        function makePopup(lat, lng) {
            return '<div style="min-width:200px;font-family:\'DM Sans\',sans-serif">' +
                '<div style="display:flex;align-items:center;gap:8px;margin-bottom:8px">' +
                '<div style="width:10px;height:10px;border-radius:50%;background:' + PIN_COLOR + ';' +
                'box-shadow:0 0 0 3px ' + PIN_COLOR + '33;flex-shrink:0"></div>' +
                '<span style="font-size:14px;font-weight:700;color:' + PIN_COLOR + '">Your Location</span>' +
                '</div>' +
                '<div style="font-size:12px;color:#8a92a8;margin-bottom:6px">' +
                '<i class="fa-solid fa-location-dot" style="margin-right:4px"></i>' +
                parseFloat(lat).toFixed(5) + ',  ' + parseFloat(lng).toFixed(5) +
                '</div>' +
                '<div style="font-size:11px;color:#475569">' +
                '<i class="fa-solid fa-arrows-up-down-left-right" style="margin-right:4px"></i>' +
                'Drag pin or click map to adjust' +
                '</div></div>';
        }

        var marker = null;
        var latEl = document.getElementById('volunteerLat');
        var lngEl = document.getElementById('volunteerLng');

        function placePin(lat, lng) {
            if (latEl) latEl.value = lat.toFixed(6);
            if (lngEl) lngEl.value = lng.toFixed(6);

            var h = document.getElementById('volunteerMapHint');
            if (h) h.style.display = 'none';

            if (marker) {
                marker.setLatLng([lat, lng]);
                marker.setPopupContent(makePopup(lat, lng));
            } else {
                marker = L.marker([lat, lng], { icon: makePinIcon(), draggable: true })
                    .addTo(map)
                    .bindPopup(makePopup(lat, lng), { maxWidth: 260, className: 'ders-popup' })
                    .openPopup();

                marker.on('dragend', function () {
                    var pos = marker.getLatLng();
                    placePin(pos.lat, pos.lng);
                });
            }
        }

        map.on('click', function (e) { placePin(e.latlng.lat, e.latlng.lng); });

        this._maps['volunteerMap'] = map;
        setTimeout(function () { map.invalidateSize(); }, 400);
        console.log('[mapInterop] volunteerMap initialised ✅');
    },

    // ── Pan volunteer map to a position (used by GPS auto-detect) ───
    setVolunteerMapPin: function (lat, lng) {
        var map = this._maps['volunteerMap'];
        if (!map) return;

        map.setView([lat, lng], 13);

        var latEl = document.getElementById('volunteerLat');
        var lngEl = document.getElementById('volunteerLng');
        if (latEl) latEl.value = lat.toFixed(6);
        if (lngEl) lngEl.value = lng.toFixed(6);

        // Re-use the existing click handler by firing a synthetic pin placement
        map.eachLayer(function (layer) {
            if (layer instanceof L.Marker) {
                layer.setLatLng([lat, lng]);
            }
        });
    },

    // ── Mini map for Update Team Location modal ──────────────────────
    // Uses the same CARTO tiles, Syria bounds, and pin style as DersLiveMap.
    initLocationMap: function (lat, lng) {
        var el = document.getElementById('teamLocationMap');
        if (!el) { console.error('[mapInterop] teamLocationMap div not found'); return; }

        if (this._maps['teamLocationMap']) {
            this._maps['teamLocationMap'].remove();
            delete this._maps['teamLocationMap'];
        }

        // ── Inject popup + pin styles once (mirrors live-map.js) ──────
        if (!document.getElementById('ders-location-map-styles')) {
            var s = document.createElement('style');
            s.id = 'ders-location-map-styles';
            s.textContent = [
                '@keyframes ders-loc-pulse{',
                '0%,100%{box-shadow:0 0 0 3px rgba(59,130,246,.5),0 2px 8px rgba(0,0,0,.5)}',
                '50%{box-shadow:0 0 0 9px rgba(59,130,246,0),0 2px 8px rgba(0,0,0,.5)}}',
                '.ders-loc-pin{background:transparent!important;border:none!important}',
                '.ders-loc-popup .leaflet-popup-content-wrapper{',
                '  background:#0f172a;border:1px solid #1e293b;border-radius:12px;',
                '  box-shadow:0 8px 32px rgba(0,0,0,.6);color:#f1f5f9}',
                '.ders-loc-popup .leaflet-popup-tip{background:#0f172a}',
                '.ders-loc-popup .leaflet-popup-close-button{color:#64748b!important}',
            ].join('');
            document.head.appendChild(s);
        }

        var dark = document.documentElement.dataset.theme !== 'light';
        var tiles = dark
            ? 'https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png'
            : 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png';

        // Syria bounds — same constants as DersLiveMap
        var SYRIA_CENTER = [34.8, 38.9];
        var SYRIA_BOUNDS = [[32.3, 35.6], [37.3, 42.4]];

        // If the team has a real position use it; otherwise fall back to Syria center
        var hasPos = (lat !== 0 || lng !== 0);
        var startCenter = hasPos ? [lat, lng] : SYRIA_CENTER;
        var startZoom = hasPos ? 10 : 7;

        var map = L.map('teamLocationMap', {
            center: startCenter,
            zoom: startZoom,
            minZoom: 5,
            maxZoom: 17,
            maxBounds: SYRIA_BOUNDS,
            maxBoundsViscosity: 0.9,
            zoomControl: true,
        });

        L.tileLayer(tiles, {
            attribution: '© <a href="https://carto.com">CARTO</a> · © <a href="https://openstreetmap.org">OpenStreetMap</a>',
            subdomains: 'abcd',
            maxZoom: 19,
        }).addTo(map);

        // ── Custom pin icon (matches DersLiveMap team-pin style) ───────
        function makePinIcon() {
            return L.divIcon({
                html: '<div style="' +
                    'width:34px;height:34px;border-radius:50%;' +
                    'background:#0f172a;border:3px solid #3b82f6;' +
                    'display:flex;align-items:center;justify-content:center;' +
                    'font-size:14px;color:#3b82f6;' +
                    'animation:ders-loc-pulse 1.8s ease infinite;' +
                    '">' +
                    '<i class="fa-solid fa-location-dot"></i>' +
                    '</div>',
                className: 'ders-loc-pin',
                iconSize: [34, 34],
                iconAnchor: [17, 17],
                popupAnchor: [0, -22],
            });
        }

        var latEl = document.getElementById('teamLocationLat');
        var lngEl = document.getElementById('teamLocationLng');

        // ── Place initial marker if team already has a position ────────
        var marker = null;
        if (hasPos) {
            marker = L.marker([lat, lng], { icon: makePinIcon(), draggable: true }).addTo(map);
            marker.bindPopup(
                '<div style="font-family:\'DM Sans\',sans-serif;font-size:12px;color:#94a3b8">' +
                '<i class="fa-solid fa-location-dot" style="color:#3b82f6;margin-right:6px"></i>' +
                'Drag or click map to update</div>',
                { className: 'ders-loc-popup', maxWidth: 220 }
            );
            if (latEl) latEl.value = lat.toFixed(6);
            if (lngEl) lngEl.value = lng.toFixed(6);
            if (window._syncLocDisplay) window._syncLocDisplay();
        }

        // ── Helpers to update/place the marker ─────────────────────────
        function placeMarker(clat, clng) {
            if (latEl) latEl.value = clat.toFixed(6);
            if (lngEl) lngEl.value = clng.toFixed(6);
            if (window._syncLocDisplay) window._syncLocDisplay();
            if (marker) {
                marker.setLatLng([clat, clng]);
            } else {
                marker = L.marker([clat, clng], { icon: makePinIcon(), draggable: true }).addTo(map);
                marker.on('dragend', function () {
                    var pos = marker.getLatLng();
                    placeMarker(pos.lat, pos.lng);
                });
            }
        }

        map.on('click', function (e) { placeMarker(e.latlng.lat, e.latlng.lng); });

        if (marker) {
            marker.on('dragend', function () {
                var pos = marker.getLatLng();
                placeMarker(pos.lat, pos.lng);
            });
        }

        this._maps['teamLocationMap'] = map;
        setTimeout(function () { map.invalidateSize(); }, 400);
    },

    // ── Destroy any map by id ────────────────────────────────────────
    destroyMap: function (mapId) {
        if (this._maps[mapId]) {
            this._maps[mapId].remove();
            delete this._maps[mapId];
        }
    }
};

// ── Blazor value getters for Create Team ────────────────────────────
window.getTeamLat = function () {
    return parseFloat(document.getElementById('teamLat')?.value) || 0;
};
window.getTeamLng = function () {
    return parseFloat(document.getElementById('teamLng')?.value) || 0;
};

// ── Blazor value getters for Report Disaster ────────────────────────
window.getDisasterLat = function () {
    return parseFloat(document.getElementById('disasterLat')?.value) || 0;
};
window.getDisasterLng = function () {
    return parseFloat(document.getElementById('disasterLng')?.value) || 0;
};

// ── Blazor value getters for Register Volunteer ─────────────────────
window.getVolunteerLat = function () {
    return parseFloat(document.getElementById('volunteerLat')?.value) || 0;
};
window.getVolunteerLng = function () {
    return parseFloat(document.getElementById('volunteerLng')?.value) || 0;
};

// ── Blazor value getters for Update Team Location ───────────────────
// -- Coordinate display sync (called after pin placement) --
window._syncLocDisplay = function () {
    var lat = document.getElementById('teamLocationLat')?.value;
    var lng = document.getElementById('teamLocationLng')?.value;
    var el = document.getElementById('locCoordDisplay');
    if (el && lat && lng) {
        el.textContent = parseFloat(lat).toFixed(5) + ',  ' + parseFloat(lng).toFixed(5);
        el.style.color = '#3b82f6';
    }
};

window.getTeamLocationLat = function () {
    return parseFloat(document.getElementById('teamLocationLat')?.value) || 0;
};
window.getTeamLocationLng = function () {
    return parseFloat(document.getElementById('teamLocationLng')?.value) || 0;
};

// ── Blazor InputFile click helper ───────────────────────────────────
// Must receive the ElementReference directly from Blazor so the browser
// keeps the element registered in Blazor's internal _blazorFilesById map.
// Do NOT use getElementById() or eval() — that bypasses the registration.
window.BlazorInputFileInterop = {
    click: function (element) {
        element.click();
    }
};