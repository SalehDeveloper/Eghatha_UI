// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class DisasterTimelineEvent
    { 
        public Guid Id { get; set; }

        public string EventType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime OccurredAt { get; set; }
    }
}