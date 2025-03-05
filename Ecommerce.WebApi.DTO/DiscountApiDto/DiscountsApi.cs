using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.DiscountApiDto
{
    public class DiscountsApi
    {   public int Id { get; set; }   
        public decimal Percentage { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
