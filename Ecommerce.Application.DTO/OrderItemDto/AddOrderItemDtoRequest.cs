﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.OrderItemDto
{
    public class AddOrderItemDtoRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
