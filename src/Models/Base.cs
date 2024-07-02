using System.ComponentModel.DataAnnotations;

namespace RestApiSample.Models
{
    public class Base
    {
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

        [StringLength(550)]
        [Required]
        public string CreatedBy { get; set; } = null!;

        public string? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }
}