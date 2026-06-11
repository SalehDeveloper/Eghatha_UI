/* ═══════════════════════════════════════════════════════════════
   DERS — Charts  (wwwroot/js/charts.js)
   Ported from the original charts.js — zero logic changes.
   Called from Blazor via JS interop after data is ready.

   Usage from C# (Dashboard.razor):
     await JS.InvokeVoidAsync("DersCharts.initDisasterChart",
         barLabels, activeData, resolvedData);
     await JS.InvokeVoidAsync("DersCharts.initDisasterTypeChart",
         typeLabels, typeData);
   ═══════════════════════════════════════════════════════════════ */

window.DersCharts = (() => {

    let _barChart = null;
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

    function _tickColor() { return _css('--text-muted') || '#525b73'; }
    function _gridColor() { return 'rgba(255,255,255,0.05)'; }
    function _legendColor() { return _css('--text-secondary') || '#8a92a8'; }

    /* ── BAR: Active vs Resolved per disaster type ── */
    function initDisasterChart(labels, active, resolved) {
        const canvas = document.getElementById('disasterChart');
        if (!canvas || !labels || !labels.length) return;

        if (_barChart) { try { _barChart.destroy(); } catch { } _barChart = null; }

        _barChart = new Chart(canvas, {
            type: 'bar',
            data: {
                labels,
                datasets: [
                    {
                        label: 'Active',
                        data: active || labels.map(() => 0),
                        backgroundColor: 'rgba(255,77,79,0.75)',
                        borderColor: '#ff4d4f',
                        borderWidth: 1,
                        borderRadius: 4,
                    },
                    {
                        label: 'Resolved',
                        data: resolved || labels.map(() => 0),
                        backgroundColor: 'rgba(82,196,26,0.75)',
                        borderColor: '#52c41a',
                        borderWidth: 1,
                        borderRadius: 4,
                    },
                ],
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        display: true,
                        position: 'top',
                        align: 'end',
                        labels: {
                            color: _legendColor(),
                            font: { size: 11, family: 'DM Sans, sans-serif' },
                            boxWidth: 10,
                            padding: 14,
                        },
                    },
                    tooltip: _tooltip(),
                },
                scales: {
                    x: {
                        grid: { color: _gridColor() },
                        ticks: {
                            color: _tickColor(),
                            font: { size: 10 },
                            maxRotation: 35,
                            callback(val) {
                                const label = this.getLabelForValue(val);
                                return label.length > 12 ? label.slice(0, 12) + '…' : label;
                            },
                        },
                    },
                    y: {
                        grid: { color: _gridColor() },
                        ticks: { color: _tickColor(), font: { size: 11 }, stepSize: 1 },
                        beginAtZero: true,
                    },
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