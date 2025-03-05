using AutoMapper;
using Ecommerce.Application.DTO.OrderDto;
using Ecommerce.Application.DTO.OrderItemDto;
using Ecommerce.Domain;
using Ecommerce.WebApi.DTO.OrderItemApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.MapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            #region AddOrder
            CreateMap<AddOrderDtoRequest, Order>();

            CreateMap<Order, AddOrderDtoResponse>();
            #endregion

            #region GetOrderById
            CreateMap<GetOrderByIdDtoRequest, Order>();

            CreateMap<Order, GetOrderByIdDtoResponse>();
            #endregion

            #region GetAllOrders
            CreateMap<GetAllOrdersDtoRequest, Order>();

            CreateMap<Order, OrdersResponse>();

            CreateMap<List<Order>, GetAllOrdersDtoResponse>()
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src));
            #endregion

            #region UpdateOrder
            CreateMap<UpdateOrderDtoRequest, Order>();

            CreateMap<Order, UpdateOrderDtoResponse>();
            #endregion

            #region DeleteOrder
            CreateMap<DeleteOrderDtoRequest, Order>();

            CreateMap<Order, DeleteOrderDtoResponse>();
            #endregion

            #region GetOrderItem
            CreateMap<OrderItem, GetOrderItemDtoResponse>()
           .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
            #endregion
        }
    }
}