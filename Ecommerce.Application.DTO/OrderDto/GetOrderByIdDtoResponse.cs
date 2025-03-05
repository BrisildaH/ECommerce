using Ecommerce.Domain;

namespace Ecommerce.Application.DTO.OrderDto
{
    public class GetOrderByIdDtoResponse
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }

    }
}
