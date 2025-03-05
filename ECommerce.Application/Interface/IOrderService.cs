using Ecommerce.Application.DTO.OrderDto;
using Ecommerce.Application.DTO.OrderItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interface
{
    public interface IOrderService
    {
        Task<GetOrderByIdDtoResponse> GetOrderById(int id);
        Task<GetAllOrdersDtoResponse> GetAllOrders();
        Task<AddOrderDtoResponse> AddOrder(AddOrderDtoRequest addOrderDto);
        Task<DeleteOrderDtoResponse> DeleteOrder(int id);
        Task<List<GetOrderByIdDtoResponse>> GetFilteredOrders(int? userId, int? productId, DateTime? startDate,
                                                                           DateTime? endDate, int pageNumber, int pageSize);
        Task<List<GetOrderItemDtoResponse>> GetOrderItemsByOrderId(int orderId);

    }
}
