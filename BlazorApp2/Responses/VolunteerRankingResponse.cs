// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class VolunteerRankingResponse
    {
        public Guid VolunteerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int TotalMissions { get; set; }
        public int TotalScore { get; set; }
        public double AverageScore { get; set; }
        public int Rank { get; set; }
    }
}