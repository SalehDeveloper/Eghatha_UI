// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class TeamDetailResponse : TeamResponse
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}