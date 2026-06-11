// ═══════════════════════════════════════════════════════════════
// REQUEST MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Requests
{
    public class AddTeamResourceRequest
    {
        public string ResourceType { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}

// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

