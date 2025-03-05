using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.DiscountDto
{
    public class UpdateDiscountDtoRequest
    {
        public int Id { get; set; } //shtim te testet
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public int ProductId { get; set; } // FK
        public int UserId { get; set; } // FK
    }
}
