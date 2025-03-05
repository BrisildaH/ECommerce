using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [ForeignKey("RoleId")]

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public ICollection<Order> Orders { get; set; }
        //public virtual ICollection<Discount> Discounts { get; set; }
    }
}
