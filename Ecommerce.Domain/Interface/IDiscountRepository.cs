namespace Ecommerce.Domain.Interface
{
    public interface IDiscountRepository
    {
        Task<Discount> GetDiscountByProductId(int productId);
        Task<Discount> GetDiscountByUserId(int userId);
        Task<Discount> GetDiscountByProductUserId(int productId, int userId);
        Task<Discount> GetDiscountById(int id);
        Task<Discount> GetDiscountByDescription(string description);
        IQueryable<Discount> GetAllDiscounts();
        Task AddDiscount(Discount discount);
        Task UpdateDiscount(Discount discount);
        Task DeleteDiscount(int discountId);
    }
}
