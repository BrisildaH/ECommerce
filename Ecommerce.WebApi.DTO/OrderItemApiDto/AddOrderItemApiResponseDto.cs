namespace Ecommerce.WebApi.DTO.OrderItemApiDto
{
    public class AddOrderItemApiResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
