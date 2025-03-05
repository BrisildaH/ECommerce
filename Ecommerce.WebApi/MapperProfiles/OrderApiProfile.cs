using AutoMapper;
using Ecommerce.Application.DTO.OrderDto;
using Ecommerce.Application.DTO.OrderItemDto;
using Ecommerce.WebApi.DTO.OrderApiDto;
using Ecommerce.WebApi.DTO.OrderItemApiDto;

namespace Ecommerce.WebApi.MapperProfiles
{
    public class OrderApiProfile : Profile
    {
        public OrderApiProfile()
        {
            #region AddOrder
            CreateMap<AddOrderApiRequestDto, AddOrderDtoRequest>();
            CreateMap<OrderItemApiRequest, OrderItemRequest>();

            CreateMap<AddOrderApiResponseDto, AddOrderDtoResponse>();
            #endregion


            #region GetOrderById
            CreateMap<GetOrderByIdApiRequestDto, GetOrderByIdDtoRequest>();

            CreateMap<GetOrderByIdDtoResponse, GetOrderByIdApiResponseDto>();
            #endregion

            #region GetAllOrders
            CreateMap<GetAllOrdersApiRequestDto, GetAllOrdersDtoRequest>();

            CreateMap<OrdersResponse, OrdersApiResponse>();
            CreateMap<GetAllOrdersDtoResponse, GetAllOrdersApiResponseDto>();//ate qe kemi, ne ate qe do mapohet.
            #endregion

            #region UpdateOrder
            CreateMap<UpdateOrderApiRequestDto, UpdateOrderDtoRequest>();

            CreateMap<UpdateOrderDtoResponse, UpdateOrderApiResponseDto>(); 
            #endregion

            #region DeleteOrder
            CreateMap<DeleteOrderApiRequestDto, DeleteOrderDtoRequest>();

            CreateMap<DeleteOrderApiResponseDto, DeleteOrderDtoResponse>();
            #endregion

            #region GetOrderItem

            CreateMap<GetOrderItemDtoResponse, OrderItemApiResponseDto>();
            #endregion
        }
    }
}
