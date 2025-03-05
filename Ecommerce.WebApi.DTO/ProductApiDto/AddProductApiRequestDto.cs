using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.ProductApiDto
{
    public class AddProductApiRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPublic { get; set; }
        public bool IsAvailable { get; set; }
        public int Stock { get; set; }

    }
}
