


namespace BlazorApp2.Responses
{
    public sealed record VolunteerDisasterResponse(Guid DisasterId, string Title,
  string City,
  string Province,
  double Latitude,
  double Longitude,
  string Type,
  string Status,
 DateTimeOffset StartTime);

}
