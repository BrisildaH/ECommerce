﻿using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.OrderApiDto
{
    public class GetAllOrdersApiResponseDto
    {
        public List<OrdersApiResponse> Orders { get; set; }
       
    }
}
