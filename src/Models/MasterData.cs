using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestApiSample.Models
{
    [Index(nameof(Name), IsUnique = true)]
    [Table("masterData")]
    public class MasterData : Base
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(550)]
        [Required]
        public string Name { get; set; } = null!;

        [StringLength(550)]
        [Required]
        public string? Value { get; set; }
    }
}