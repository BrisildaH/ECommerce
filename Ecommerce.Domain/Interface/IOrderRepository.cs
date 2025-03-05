using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interface
{ 
    public interface IOrderRepository
    {
        Task<Order> GetOrderById(int id);
        IQueryable<Order> GetAll();
        Task AddOrder(Order order);
        Task DeleteOrder(int id);
        Task<List<Order>> GetFilteredOrders(int? userId, int? productId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);


    }
}
