using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestApiSample.Models
{
    [Index(nameof(ProductId))]
    [Table("warehouse")]
    public class WareHouse : Base
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Amount { get; set; }

        public virtual Product Product { get; set; } = null!;
        public int ProductId { get; set; }


    }
}
