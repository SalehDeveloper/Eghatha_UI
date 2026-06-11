


namespace BlazorApp2.Responses
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }

}
