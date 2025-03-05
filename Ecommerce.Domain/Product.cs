using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Domain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsPublic { get; set; }

        public bool IsAvailable { get; set; }
        //public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    }
}
