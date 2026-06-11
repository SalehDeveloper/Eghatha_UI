


namespace BlazorApp2.Responses
{
    public record TeamDisasterResponse(Guid DisasterId,
      string Title,
      string City,
      string Province,
      double Latitude,
      double Longitude,
      string Type,
      string Status,
     DateTimeOffset StartTime);
}