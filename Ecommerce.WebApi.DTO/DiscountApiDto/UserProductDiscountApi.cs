namespace Ecommerce.WebApi.DTO.DiscountApiDto
{
    public class UserProductDiscountApi
    {
        public decimal Percentage { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
