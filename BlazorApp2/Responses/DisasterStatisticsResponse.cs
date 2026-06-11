// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class DisasterStatisticsResponse
    {
        public int TotalDisasters { get; set; }
        public int ActiveDisasters { get; set; }
        public int ResolvedDisasters { get; set; }
        public int ClosedDisasters { get; set; }
        public int PendingDisasters { get; set; }
    }
}