namespace BlazorApp2.Responses
{
    public class LoginResponse
    { 
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }= new List<string>();
    }


}
