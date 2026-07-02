// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

using System.Text.Json.Serialization;

namespace BlazorApp2.Responses
{
    public class DisasterDetailResponse : DisasterResponse
    {
        public string Description { get; set; } = string.Empty;
     public ReporterDto Reporter { get; set; }

        public DateTime? EndTime { get; set; }

        public List<TeamDto> Teams { get; set; } = new();

        public List<ResourceDto> Resources { get; set; } = new();
        public ReportDto? Report { get; set; }

        [JsonPropertyName("affectedPeople")]
        public List<AffectedPersonDto> AffectedPersons { get; set; } = new();



    }
}