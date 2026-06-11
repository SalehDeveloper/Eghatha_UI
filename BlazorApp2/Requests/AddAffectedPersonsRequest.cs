// ═══════════════════════════════════════════════════════════════
// REQUEST MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Requests
{
    public class AddAffectedPersonsRequest
    {
        public List<AffectedPersonDto> Persons { get; set; } = new();
    }
}


