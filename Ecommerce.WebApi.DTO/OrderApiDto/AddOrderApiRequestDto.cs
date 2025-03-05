using Ecommerce.Domain;
using Ecommerce.WebApi.DTO.OrderItemApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.OrderApiDto
{
    public class AddOrderApiRequestDto
    {  
        public List<OrderItemApiRequest> OrderItems { get; set; }
    }
  
}
