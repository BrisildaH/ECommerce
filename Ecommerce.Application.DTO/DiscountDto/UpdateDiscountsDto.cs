namespace Ecommerce.Application.DTO.DiscountDto
{
    public class UpdateDiscountsDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public int ProductId { get; set; } // FK
        public int UserId { get; set; } // FK
    }
}
