// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class TeamMemberResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsLeader { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
    }
}