using Ecommerce.Application.DTO.OrderItemDto;
using Ecommerce.WebApi.DTO.OrderApiDto;

namespace Ecommerce.Application.DTO.OrderDto
{
    public class AddOrderDtoRequest
    {
        public int UserId { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; }
    }
  
}
