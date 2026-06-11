// ═══════════════════════════════════════════════════════════════
// REQUEST MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Requests
{
    public sealed record EvaluateVolunteerRequest(int CommitmentScore,
      int SkillScore,
      int SafetyScore,
      int TeamWorkScore,
      int InitiativeScore,
      string? Notes);
}


