using AutoMapper;
using Ecommerce.Application.DTO.OrderDto;
using Ecommerce.Application.DTO.ProductDto;
using Ecommerce.WebApi.DTO.OrderApiDto;
using Ecommerce.WebApi.DTO.ProductApiDto;

namespace Ecommerce.WebApi.MapperProfiles
{
    public class ProductApiProfile : Profile
    {
        public ProductApiProfile()
        {
            #region AddProduct
            CreateMap<AddProductApiRequestDto, AddProductDtoRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<AddProductApiResponseDto, AddProductDtoResponse>()

                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            #endregion

            // duhet te krijoj mappim per te gjitha DTOs
            #region GetProductById
            CreateMap<GetProductByIdApiRequestDto, GetProductByIdDtoRequest>();

            CreateMap<GetProductByIdDtoResponse, GetProductByIdApiResponseDto>();
            #endregion

            #region GetProductForUsers
            CreateMap<GetProductByIdApiRequestDto, GetProductByIdDtoRequest>();

            CreateMap<GetProductForUsersDtoResponse, GetProductForUserApiResponseDto>();
            #endregion

            #region GetAllProducts
            CreateMap<GetAllProductsApiRequestDto, GetAllProductsDtoRequest>();

            CreateMap<ProductsResponse, ProductsApiResponse>();

            CreateMap<GetAllProductsDtoResponse, GetAllProductsApiResponseDto>();

            #endregion

            #region UpdateProduct
            CreateMap<UpdateProductApiRequestDto, UpdateProductDtoRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<UpdateProductApiResponseDto, UpdateProductDtoResponse>()

                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            #endregion

            #region DeleteProduct
            CreateMap<DeleteProductApiRequestDto, DeleteProductDtoRequest>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<DeleteProductApiResponseDto, DeleteProductDtoResponse>();
            #endregion
        }
    }
}