


namespace BlazorApp2.Responses
{
    public record TeamMapDto(
    Guid Id,
    string Name,
    string Speciality,
    string Status,
    double Latitude,
    double Longitude,
    bool IsLiveLocation,
    Guid? AssignedDisasterId
);
}