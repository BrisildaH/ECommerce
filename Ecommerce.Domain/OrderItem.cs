using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public decimal TotalRowAmount { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
       

    }
}
