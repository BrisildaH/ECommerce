using AutoMapper;
using Ecommerce.Application.DTO.ProductDto;
using Ecommerce.Domain;

namespace Ecommerce.Application.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            #region AddProduct

            CreateMap<AddProductDtoRequest, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<Product, AddProductDtoResponse>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));
            #endregion

            // duhet te krijoj mappim per te gjitha DTOs
            #region GetProductById
            CreateMap<GetProductByIdDtoRequest, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<Product, GetProductByIdDtoResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            #endregion

            #region GettAllProducts
            CreateMap<GetAllProductsDtoRequest, Product>();

            CreateMap<Product, ProductsResponse>();

            CreateMap<List<Product>, GetAllProductsDtoResponse>()
           .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));
            #endregion


            #region UpdateProduct
            CreateMap<UpdateProductDtoRequest, Product>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
               .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
               .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<Product, UpdateProductDtoResponse>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
               .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
               .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            #endregion

            #region DeleteProduct
            CreateMap<DeleteProductDtoRequest, Product>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<Product, DeleteProductDtoResponse>();

            #endregion

        }
    }
}
