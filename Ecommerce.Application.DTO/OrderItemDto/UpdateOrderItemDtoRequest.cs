using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.OrderItemDto
{
    public class UpdateOrderItemDtoRequest
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
