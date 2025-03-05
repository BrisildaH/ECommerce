using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.OrderDto
{
    public class GetAllOrdersDtoResponse
    {
        public List<OrdersResponse> Orders { get; set; }

    }
}
