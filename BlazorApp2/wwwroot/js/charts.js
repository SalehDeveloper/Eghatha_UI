/* ═══════════════════════════════════════════════════════════════
   DERS — Charts  (wwwroot/js/charts.js)
   Called from Blazor via JS interop after data is ready.

   Usage from C# (Dashboard.razor):
     await JS.InvokeVoidAsync("DersCharts.initDisasterChart",
         statusLabels, statusData);
     await JS.InvokeVoidAsync("DersCharts.initDisasterTypeChart",
         typeLabels, typeData);
   ═══════════════════════════════════════════════════════════════ */

window.DersCharts = (() => {

    let _statusChart = null;
    let _donutChart = null;

    function _css(name) {
        return getComputedStyle(document.documentElement)
            .getPropertyValue(name).trim();
    }

    function _tooltip() {
        return {
            backgroundColor: '#1a1e2a',
            titleColor: '#e8eaf0',
            bodyColor: '#8a92a8',
            borderColor: 'rgba(255,255,255,0.08)',
            borderWidth: 1,
            padding: 12,
            cornerRadius: 8,
        };
    }

    function _legendColor() { return _css('--text-secondary') || '#8a92a8'; }

    /* ── DOUGHNUT: Disaster status distribution ── */
    function initDisasterChart(labels, data) {
        const canvas = document.getElementById('disasterChart');
        if (!canvas || !labels || !labels.length || !data || !data.length) return;

        if (_statusChart) { try { _statusChart.destroy(); } catch { } _statusChart = null; }

        const STATUS_PALETTE = {
            'Reported': { bg: 'rgba(250,173,20,0.75)', border: '#faad14' },
            'InProgress': { bg: 'rgba(24,144,255,0.75)', border: '#1890ff' },
            'Resolved': { bg: 'rgba(82,196,26,0.75)', border: '#52c41a' },
            'Closed': { bg: 'rgba(82,130,246,0.75)', border: '#5282f6' },
            'Archived': { bg: 'rgba(82,92,108,0.75)', border: '#525c6c' },
            'Cancelled': { bg: 'rgba(255,77,79,0.75)', border: '#ff4d4f' },
        };

        const bgColors = labels.map(l => (STATUS_PALETTE[l] || { bg: 'rgba(138,146,168,0.75)' }).bg);
        const borderColors = labels.map(l => (STATUS_PALETTE[l] || { border: '#8a92a8' }).border);

        _statusChart = new Chart(canvas, {
            type: 'doughnut',
            data: {
                labels,
                datasets: [{
                    data,
                    backgroundColor: bgColors,
                    borderColor: borderColors,
                    borderWidth: 2,
                    hoverOffset: 6,
                }],
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: '62%',
                plugins: {
                    legend: {
                        display: true,
                        position: 'bottom',
                        labels: {
                            color: _legendColor(),
                            font: { size: 11, family: 'DM Sans, sans-serif' },
                            boxWidth: 10,
                            padding: 10,
                        },
                    },
                    tooltip: _tooltip(),
                },
            },
        });
    }

    /* ── DOUGHNUT: Disaster type distribution ── */
    function initDisasterTypeChart(labels, data) {
        const canvas = document.getElementById('typeChart');
        if (!canvas || !labels || !labels.length || !data || !data.length) return;

        if (_donutChart) { try { _donutChart.destroy(); } catch { } _donutChart = null; }

        const PALETTE = [
            '#ff4d4f', '#1890ff', '#faad14', '#52c41a',
            '#7232d5', '#08979c', '#f97316', '#ec4899',
            '#06b6d4', '#84cc16',
        ];

        _donutChart = new Chart(canvas, {
            type: 'doughnut',
            data: {
                labels,
                datasets: [{
                    data,
                    backgroundColor: labels.map((_, i) => PALETTE[i % PALETTE.length] + 'bb'),
                    borderColor: labels.map((_, i) => PALETTE[i % PALETTE.length]),
                    borderWidth: 2,
                    hoverOffset: 6,
                }],
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                cutout: '62%',
                plugins: {
                    legend: {
                        display: true,
                        position: 'bottom',
                        labels: {
                            color: _legendColor(),
                            font: { size: 11, family: 'DM Sans, sans-serif' },
                            boxWidth: 10,
                            padding: 8,
                        },
                    },
                    tooltip: _tooltip(),
                },
            },
        });
    }

    return { initDisasterChart, initDisasterTypeChart };

})();
