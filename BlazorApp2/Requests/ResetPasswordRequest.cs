using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Requests
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Code { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
