// ═══════════════════════════════════════════════════════════════
// REQUEST MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Requests
{
    public class UpdateTeamRequest
    {
        public string? Name { get; set; } = string.Empty;
        public string? Speciality { get; set; } = string.Empty;
        public string? Province { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
    }
}

// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

