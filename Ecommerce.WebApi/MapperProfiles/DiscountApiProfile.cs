using AutoMapper;
using Ecommerce.Application.DTO;
using Ecommerce.Application.DTO.DiscountDto;
using Ecommerce.WebApi.DTO;
using Ecommerce.WebApi.DTO.DiscountApiDto;

namespace Ecommerce.WebApi.MapperProfiles
{
    public class DiscountApiProfile : Profile
    {
        public DiscountApiProfile()
        {

            #region AddDiscount
            CreateMap<AddDiscountApiRequestDto, AddDiscountDtoRequest>(); //duke qene se emertimi i variablave eshte i njejte

            CreateMap<AddDiscountApiResponseDto, AddDiscountDtoResponse>();
            #endregion

            #region AddDiscountUP
            CreateMap<AddDiscountsApiRequestDto, AddDiscountsDtoRequest>();
            CreateMap<UserProductDiscountApi, DiscountsDto>();
            CreateMap<AddDiscountApiResponseDto, AddDiscountDtoResponse>();
            #endregion

            #region UpdateDiscountUP

            CreateMap<UpdateDiscountsApiRequestDto, UpdateDiscountsDtoRequest>();
            CreateMap<DiscountsApi, UpdateDiscountsDto>();
            CreateMap<UpdateDiscountApiResponseDto, UpdateDiscountDtoResponse>();

            #endregion

            #region GetDiscountById
            CreateMap<GetDiscountByIdApiRequestDto, GetDiscountByIdDtoRequest>();
            CreateMap<GetDiscountByIdDtoResponse, GetDiscountByIdApiResponseDto>(); 

            #endregion

            #region GetAllDiscounts
            CreateMap<GetAllDiscountsApiRequestDto, GetAllDiscountsDtoRequest>();
            CreateMap<DiscountsDto, DiscountsApiResponse>();
            CreateMap<GetAllDiscountsDtoResponse, GetAllDiscountsApiResponseDto>(); 


            #endregion

            #region DeleteDiscount
            CreateMap<DeleteDiscountApiRequestDto, DeleteDiscountDtoRequest>();
            CreateMap<DeleteDiscountApiResponseDto, DeleteDiscountDtoResponse>();

            #endregion

            #region UpdateDiscount

            CreateMap<UpdateDiscountApiRequestDto, UpdateDiscountDtoRequest>();
            CreateMap<UpdateDiscountApiResponseDto, UpdateDiscountDtoResponse>();

            #endregion
        }
    }
}
