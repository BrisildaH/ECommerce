using Ecommerce.Application.DTO.OrderItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.OrderDto
{
    public class UpdateOrderDtoRequest
    {
        public int UserId { get; set; } // per t'u pare cfare mund te bej update...
        public int OrderItemId { get; set; }

    }
}
