using Ecommerce.WebApi.DTO.OrderItemApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.OrderApiDto
{
    public class UpdateOrderApiRequestDto
    { public int UserId { get; set; }
        public int OrderItemId { get; set; }
    }
}
