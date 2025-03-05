using AutoMapper;
using Ecommerce.Application.DTO;
using Ecommerce.Application.DTO.DiscountDto;
using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.MapperProfiles
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            #region AddDiscount
            CreateMap<AddDiscountDtoRequest, Discount>();

            CreateMap<Discount, AddDiscountDtoResponse>()
               .ForMember(dest => dest.DiscountId, opt => opt.MapFrom(src => src.Id));

            #endregion

            #region AddDiscountUP
            CreateMap<AddDiscountsDtoRequest, Discount>();
            //CreateMap<List<DiscountsDto>, List<Discount>>();

            CreateMap<Discount, AddDiscountDtoResponse>()
               .ForMember(dest => dest.DiscountId, opt => opt.MapFrom(src => src.Id));

            #endregion


            #region UpdateDiscountUP
            CreateMap<UpdateDiscountsDtoRequest, Discount>();

            CreateMap<Discount, UpdateDiscountDtoResponse>();
            #endregion

            #region GetDiscountById
            CreateMap<GetDiscountByIdDtoRequest, Discount>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DiscountId));

            CreateMap<Discount, GetDiscountByIdDtoResponse>();
            #endregion

            #region GetAllDiscounts
            CreateMap<GetAllDiscountsDtoRequest, Discount>();

            CreateMap<Discount, DiscountsDto>();

            CreateMap<List<Discount>, GetAllDiscountsDtoResponse>()
                 .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => src));
            #endregion

            #region UpdateDiscount
            CreateMap<UpdateDiscountDtoRequest, Discount>()
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Percentage));

            CreateMap<Discount, UpdateDiscountDtoResponse>();
            #endregion

            #region DeleteDiscount
            CreateMap<DeleteDiscountDtoRequest, Discount>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DiscountId));

            CreateMap<Discount, DeleteDiscountDtoResponse>();
            #endregion
        }
    }
}
