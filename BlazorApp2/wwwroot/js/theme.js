/* ═══════════════════════════════════════════════════════════════
   DERS — Theme Manager  (wwwroot/js/theme.js)
   Persists dark/light preference in localStorage.
   Called from Blazor via JS interop.
   ═══════════════════════════════════════════════════════════════ */
window.DersTheme = {

    /* Apply theme to <html data-theme="..."> and save preference */
    apply(theme) {
        document.documentElement.setAttribute('data-theme', theme);
        try { localStorage.setItem('ders-theme', theme); } catch { }
    },

    /* Read saved preference — returns 'dark' if nothing saved */
    getSaved() {
        try { return localStorage.getItem('ders-theme') || 'dark'; } catch { return 'dark'; }
    },


    /* Apply saved preference immediately (call on page load) */
    init() {
        const saved = this.getSaved();
        document.documentElement.setAttribute('data-theme', saved);
        return saved;   // returned to Blazor so it can sync its boolean
    },

    /* Smooth-scroll an element into view — used by the AI chat page */
    scrollIntoView(elementId) {
        document.getElementById(elementId)?.scrollIntoView({ behavior: 'smooth', block: 'end' });
    }
};

/* Apply theme BEFORE first render to avoid flash */
DersTheme.init();