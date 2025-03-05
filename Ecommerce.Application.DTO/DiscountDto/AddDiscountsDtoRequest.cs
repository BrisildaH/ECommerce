using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.DiscountDto
{
    public class AddDiscountsDtoRequest
    {
        public List<DiscountsDto> Discounts { get; set; }
    }
}
