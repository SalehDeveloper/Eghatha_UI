// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class CreateDisasterResponse
    {
        public Guid Id { get; set; }

        public string Status { get; set; }
     public List<RecommendedTeamsResponse> RecommendedTeams { get; set; } = new();
     public List<RecommendedVolunteerResponse> RecommendedVolunteers { get; set; } = new();
    }

    public sealed record RecommendedTeamsResponse(Guid TeamId,
   string TeamName,
   string Speciality,
   string Province , 
   string City , 
   double DistanceKm,
   double DurationMinutes,
   double Score,
   bool IsLiveLocation);

    public sealed record RecommendedVolunteerResponse(Guid VolunteerId,
    string Speciality,
    double DistanceKm,
    double DurationMinutes,
    double Score);
}