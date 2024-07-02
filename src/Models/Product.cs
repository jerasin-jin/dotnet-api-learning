using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace RestApiSample.Models
{
    [Index(nameof(Active))]
    [Index(nameof(Name), IsUnique = true)]
    [Table("products")]
    public class Product : Base
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(550)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Price { get; set; }

        public string? Img { get; set; }

        public bool Active { get; set; } = true;

        [JsonIgnore]
        public virtual WareHouse WareHouse { get; set; } = null!;


        public ICollection<SaleOrder> SaleOrders { get; } = new List<SaleOrder>();
    }
}
