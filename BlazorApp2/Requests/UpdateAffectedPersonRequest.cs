// ═══════════════════════════════════════════════════════════════
// REQUEST MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Requests
{
    public class UpdateAffectedPersonRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}

// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

