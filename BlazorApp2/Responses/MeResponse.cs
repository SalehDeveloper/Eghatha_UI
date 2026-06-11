namespace BlazorApp2.Responses
{
    public class MeResponse
    {
        public Guid Id { get; set; }
        public  string Email { get; set; }
        public IList<string> Roles { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
