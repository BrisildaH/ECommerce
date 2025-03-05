using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
        //public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
    }
}
