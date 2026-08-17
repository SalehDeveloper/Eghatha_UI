


namespace BlazorApp2.Responses
{
    public record SpamCheckResponse(
       bool IsSpam,
       Guid? MatchedDisasterId,
       double Confidence,
       string Reasoning);

}
