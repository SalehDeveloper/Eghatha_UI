// ═══════════════════════════════════════════════════════════════
// REQUEST MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Requests
{
    public class DispathVolunteersRequest
    {
        public List<Guid> VolunteerIds { get; set; } = new();
    }
}


