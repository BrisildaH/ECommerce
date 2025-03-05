using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Ecommerce.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.AddRangeAsync(orderItem);
            await _context.SaveChangesAsync(); 
        }

        public IQueryable <OrderItem>GetAllItemOrders()
        {
          return _context.OrderItems;
        }
        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Product)  
                .ToListAsync();
        }

    }
}
