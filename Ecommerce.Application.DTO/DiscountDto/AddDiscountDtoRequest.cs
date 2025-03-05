using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.DiscountDto
{
    public class AddDiscountDtoRequest
    {
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
