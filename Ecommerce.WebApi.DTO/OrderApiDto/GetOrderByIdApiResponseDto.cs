﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.OrderApiDto
{
    public class GetOrderByIdApiResponseDto
    {
      
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
    }
}
