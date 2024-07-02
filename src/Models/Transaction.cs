using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace RestApiSample.Models
{
    [Table("transactions")]
    public class Transaction : Base
    {

        public int Id { get; set; }

        [Key, Required, StringLength(550)]
        public string TransactionId { get; set; } = null!;

        [Required]
        public int TotalAmount { get; set; }

        [Required]
        public int TotalPrice { get; set; }


        public List<SaleOrder> SaleOrder { get; set; } = null!;
    }

}