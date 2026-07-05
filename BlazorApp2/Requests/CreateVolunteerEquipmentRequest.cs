namespace BlazorApp2.Requests
{
    public record CreateVolunteerEquipmentRequest(
   string Name,
   string Category,
   int Quantity);
}
