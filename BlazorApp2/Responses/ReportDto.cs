// ═══════════════════════════════════════════════════════════════
// RESPONSE MODELS
// ═══════════════════════════════════════════════════════════════

namespace BlazorApp2.Responses
{
    public class ReportDto
    {
       public Guid Id { get; set; }
        public string Summary { get; set; }
        public string PdfUrl{ get; set; }
        public DateTimeOffset IssuedAt { get; set; }
    }
    
 
}