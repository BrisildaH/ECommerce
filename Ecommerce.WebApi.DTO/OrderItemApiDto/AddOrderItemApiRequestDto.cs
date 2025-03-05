using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.OrderItemApiDto
{
    public class AddOrderItemApiRequestDto
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
