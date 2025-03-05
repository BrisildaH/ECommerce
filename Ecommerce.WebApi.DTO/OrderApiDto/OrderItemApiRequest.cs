using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.OrderApiDto
{
    public class OrderItemApiRequest
    {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        
    }
}
