using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestApiSample.Models
{

    [Index(nameof(TransactionId))]
    [Table("saleOrders")]
    public class SaleOrder : Base
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(550)]
        [Required]
        public string TransactionId { get; set; } = null!;

        [ForeignKey("TransactionId")]
        public virtual Transaction Transaction { get; set; } = null!;

        [Required]
        public int Amount { get; set; }

        public Product Product { get; set; } = null!;
        public int ProductId { get; set; }
    }

}
