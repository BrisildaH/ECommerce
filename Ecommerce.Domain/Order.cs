using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
      
    }
}
