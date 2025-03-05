using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.DiscountApiDto
{
    public class GetDiscountByIdApiResponseDto
    {
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public int ProductId { get; set; } // FK
         public int UserId { get; set; } // FK
    }
}
