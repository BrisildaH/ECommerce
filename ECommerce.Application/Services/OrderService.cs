using AutoMapper;
using Ecommerce.Application.DTO.OrderDto;
using Ecommerce.Application.DTO.OrderItemDto;
using Ecommerce.Application.Interface;
using Ecommerce.Application.Resources;
using Ecommerce.Common.Exceptions;
using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Transactions;

namespace Ecommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public OrderService(IOrderRepository orderRepository, IMapper mapper, 
            IProductRepository productRepository, IDiscountRepository discountRepository,
            IOrderItemRepository orderItemRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _orderItemRepository = orderItemRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetOrderByIdDtoResponse> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueO);
            }

            return _mapper.Map<GetOrderByIdDtoResponse>(order);
        }

        public async Task<GetAllOrdersDtoResponse> GetAllOrders()
        {
            var orders = await _orderRepository.GetAll().ToListAsync();

            return _mapper.Map<GetAllOrdersDtoResponse>(orders);
        }

        public async Task<AddOrderDtoResponse> AddOrder(AddOrderDtoRequest addOrderDto)
        {
            var userId = addOrderDto.UserId;

            if (addOrderDto.OrderItems == null || !addOrderDto.OrderItems.Any())
            {
                throw new BadRequestException(StringResourceMessage.OrderProductEmpty);
            }

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    decimal totalAmount = 0;
                    var orderItems = new List<OrderItem>();

                    foreach (var item in addOrderDto.OrderItems)
                    {
                        var product = await _productRepository.GetProductById(item.ProductId);
                        if (product == null)
                        {
                            throw new NotFoundException(StringResourceMessage.NotFoundValue);
                        }

                        if (product.Stock < item.Quantity)
                        {
                            throw new BadRequestException(StringResourceMessage.InsufficientStockProduct);
                        }

                        var discount = await _discountRepository.GetDiscountByProductUserId(product.Id, userId);

                        decimal discountAmount = discount?.Percentage ?? 0;
                        decimal finalPrice = product.Price * (1 - (discountAmount / 100));

                        //var discounts = product.Discounts.Where(d => d.Percentage > 0).ToList();
                        //decimal price = product.Price;

                        //foreach (var discount in discounts)
                        //{
                        //    price -= price * (discount.Percentage / 100);
                        //}

                        decimal totalRowAmount = item.Quantity * finalPrice;

                        totalAmount += totalRowAmount; 

                        var orderItem = new OrderItem
                        {
                            ProductId = item.ProductId,
                            //UserId = item.UserId,
                            Quantity = item.Quantity,
                            Price = finalPrice,
                            TotalRowAmount = totalRowAmount
                        };

                        orderItems.Add(orderItem);

                        product.Stock -= item.Quantity; 
                    }

                    var order = new Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.UtcNow,
                        TotalAmount = totalAmount
                    };
                    await _orderRepository.AddOrder(order);

                    foreach (var orderItem in orderItems)
                    {
                        orderItem.OrderId = order.Id; 
                        await _orderItemRepository.AddOrderItem(orderItem);
                    }

                    scope.Complete();

                    return new AddOrderDtoResponse
                    {
                        Id = order.Id
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DeleteOrderDtoResponse> DeleteOrder(int id)
        {
            var existingOrder = await _orderRepository.GetOrderById(id);

            if (existingOrder == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueO);
            }

            await _orderRepository.DeleteOrder(id);
            return _mapper.Map<DeleteOrderDtoResponse>(existingOrder);
        }

        //Admin
        public async Task<List<GetOrderByIdDtoResponse>> GetFilteredOrders(int? userId, int? productId, DateTime? startDate, 
                                                                           DateTime? endDate, int pageNumber, int pageSize)
        {
            var orders = await _orderRepository.GetFilteredOrders(userId, productId, startDate, endDate, pageNumber, pageSize);
            return _mapper.Map<List<GetOrderByIdDtoResponse>>(orders);
        }

        public async Task<List<GetOrderItemDtoResponse>> GetOrderItemsByOrderId(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueO);
            }

            var orderItems = await _orderItemRepository.GetOrderItemsByOrderId(orderId);


            return _mapper.Map<List<GetOrderItemDtoResponse>>(orderItems);
        }

    }
}

