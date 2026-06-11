using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Requests
{
    public class RequestResetModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
