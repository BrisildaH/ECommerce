using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Ecommerce.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }
        public  IQueryable<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product);
        }

        public async Task AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await GetOrderById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        //Admin
        public async Task<List<Order>> GetFilteredOrders(int? userId, int? productId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            var orderQuery = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (userId.HasValue)
            {
                orderQuery = orderQuery.Where(o => o.UserId == userId.Value);
            }

            if (productId.HasValue)
            {
                orderQuery = orderQuery.Where(o => o.OrderItems.Any(oi => oi.ProductId == productId.Value));
            }

            if (startDate.HasValue)
            {
                orderQuery = orderQuery.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                orderQuery = orderQuery.Where(o => o.OrderDate <= endDate.Value);
            }

            return await orderQuery
                .OrderByDescending(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
