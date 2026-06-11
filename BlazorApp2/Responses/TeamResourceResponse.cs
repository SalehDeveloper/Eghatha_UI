

namespace BlazorApp2.Responses
{
    public class TeamResourceResponse
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsConsumable { get; set; }
    }
}