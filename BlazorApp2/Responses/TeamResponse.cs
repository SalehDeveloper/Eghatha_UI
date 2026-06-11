// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class TeamResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? LeaderName { get; set; }
        public int MembersCount { get; set; }
        public int ActiveMembersCount { get; set; }
        public bool IsReadyForMission { get; set; }
    }
}