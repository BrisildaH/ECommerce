namespace Ecommerce.WebApi.DTO.OrderApiDto
{
    public class UpdateOrderApiResponseDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
    }
}
