using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Ecommerce.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //CRUD Methods
        public async Task<Discount> GetDiscountByProductId(int productId)
        {
            return await _context.Discounts
                                .FirstOrDefaultAsync(d => d.ProductId == productId);
        }

        public async Task<Discount> GetDiscountByUserId(int userId)
        {
            return await _context.Discounts
                                .FirstOrDefaultAsync(d => d.UserId == userId);
        }
        public async Task<Discount> GetDiscountByProductUserId(int productId, int userId)
        {
            return await _context.Discounts
                                .FirstOrDefaultAsync(d => d.ProductId == productId &&
                                                          d.UserId == userId);
        }
        public async Task<Discount> GetDiscountById(int id)
        {
            return await _context.Discounts
                                .Include(d => d.User)  
                                .Include(d => d.Product)  
                                .FirstOrDefaultAsync(d => d.Id == id);
        }
       
        public async Task<Discount> GetDiscountByDescription(string description)
        {
            return await _context.Discounts
                          .Where(p => p.Description.ToLower() == description.ToLower())
                          .FirstOrDefaultAsync();
        }

        public IQueryable<Discount>GetAllDiscounts()
        {
            return _context.Discounts
                .Include(d => d.User)  
                .Include(d => d.Product);
        }

        public async Task AddDiscount(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscount(int discountId)
        {
            var discount =  await GetDiscountById(discountId);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            }
        }

    }
}

