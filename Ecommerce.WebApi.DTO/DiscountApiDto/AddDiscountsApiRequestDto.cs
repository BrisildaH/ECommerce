using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.DiscountApiDto
{
    public class AddDiscountsApiRequestDto
    {
        public List<UserProductDiscountApi> Discounts { get; set; }
    }
}

