using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.OrderItemDto
{
    public class AddOrderItemDtoResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } //duhet ta lidh me ProductPrice
        public decimal TotalAmount { get; set; }
    }
}
