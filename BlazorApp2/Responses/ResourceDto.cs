// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class ResourceDto
    {
       public Guid Id { get; set; }
        public string ResourceType { get; set; }
        public int Sent { get; set; }
        public int Consumed { get; set; }
        public int Returned { get; set; }
        public int Damaged { get; set; }
        public string? Notes { get; set; }

    }
}