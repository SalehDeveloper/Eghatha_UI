/* ═══════════════════════════════════════════════════════════════
   DERS — Detail Map  (wwwroot/js/detail-map.js)
   Mini Leaflet map used on the DisasterDetail "Map" tab.
   Called from DisasterDetail.razor via JS interop.
   ═══════════════════════════════════════════════════════════════ */
window.DersDetailMap = (() => {

    let _map = null;
    let _marker = null;

    const TILES = {
        dark: 'https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png',
        light: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png',
    };

    function init(elementId, lat, lng, title, status) {
        /* Destroy any previous instance that might be stuck on the same element */
        if (_map) {
            try { _map.remove(); } catch { }
            _map = null; _marker = null;
        }

        const el = document.getElementById(elementId);
        if (!el) { console.warn('[DetailMap] element not found:', elementId); return; }

        const dark = document.documentElement.getAttribute('data-theme') !== 'light';

        _map = L.map(elementId, {
            center: [lat, lng],
            zoom: 13,
            zoomControl: true,
            scrollWheelZoom: false,
        });

        L.tileLayer(dark ? TILES.dark : TILES.light, {
            attribution: '© <a href="https://carto.com">CARTO</a> · © OpenStreetMap',
            subdomains: 'abcd',
        }).addTo(_map);

        /* Disaster marker */
        const color = _statusColor(status);

        _marker = L.marker([lat, lng], {
            icon: L.divIcon({
                html: `<div style="
          width:32px;height:32px;border-radius:50%;
          background:${color};border:2px solid #fff;
          display:flex;align-items:center;justify-content:center;
          box-shadow:0 2px 8px rgba(0,0,0,.4);
          font-size:14px;color:#fff">
            <i class='fa-solid fa-fire'></i>
          </div>`,
                className: '',
                iconSize: [32, 32],
                iconAnchor: [16, 16],
                popupAnchor: [0, -18],
            }),
        })
            .bindPopup(`<strong style="font-size:13px">${_esc(title)}</strong>
                <br><span style="font-size:11px;color:#8a92a8">${lat.toFixed(5)}, ${lng.toFixed(5)}</span>`)
            .addTo(_map)
            .openPopup();

        /* Flash ring to draw attention */
        const ring = L.circle([lat, lng], {
            radius: 800, color, fillColor: color, fillOpacity: 0.08, weight: 2, opacity: 0.6,
        }).addTo(_map);

        /* Fix size after Blazor renders the container */
        setTimeout(() => _map?.invalidateSize(), 150);
    }

    function _statusColor(status) {
        if (!status) return '#ff4d4f';
        const s = status.toLowerCase();
        if (s === 'resolved' || s === 'closed') return '#8c8c8c';
        if (s === 'inprogress' || s === 'in_progress') return '#1890ff';
        return '#ff4d4f';
    }

    function _esc(s) {
        return String(s ?? '').replace(/[&<>"']/g, c =>
            ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    }

    return { init };
})();