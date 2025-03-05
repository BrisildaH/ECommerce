﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.OrderItemDto
{
    public class GetOrderItemByIdDtoResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        //public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
