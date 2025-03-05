using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interface
{
    public interface IOrderItemRepository
    {
        Task AddOrderItem(OrderItem orderItem);
        public IQueryable<OrderItem> GetAllItemOrders();
        Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);
    }
}
