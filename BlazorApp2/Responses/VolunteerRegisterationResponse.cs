// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class VolunteerRegisterationResponse
    {
        public Guid Id { get; set; }
        public Guid VolunteerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string Province { get; set; }
        public string City { get; set; }
        public int YearsOfExperince { get; set; }
        public string Speciality { get; set; } = string.Empty;
        public string Cv { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? RejectionReason { get; set; }
    }
}