// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class AccountStatisticsResponse
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InActiveUsers { get; set; }
        public int TotalAdmins { get; set; }
    }
}