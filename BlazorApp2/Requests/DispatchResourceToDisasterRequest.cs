// ═══════════════════════════════════════════════════════════════
// REQUEST MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Requests
{
    public sealed record DispatchResourceToDisasterRequest(
       Guid TeamId,
       Guid ResourceId,
       int Quantity,
       string? Notes);
}


