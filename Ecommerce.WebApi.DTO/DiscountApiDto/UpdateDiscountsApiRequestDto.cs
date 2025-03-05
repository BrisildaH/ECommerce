using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.DiscountApiDto
{
    public class UpdateDiscountsApiRequestDto
    {
        public List<DiscountsApi> Discounts { get; set; }
    }
}
