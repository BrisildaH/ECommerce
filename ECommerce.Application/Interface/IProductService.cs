using Ecommerce.Application.DTO.ProductDto;
using Ecommerce.Domain;
using Ecommerce.WebApi.DTO;
using Ecommerce.WebApi.DTO.ProductApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interface
{
    public interface IProductService
    {
        Task<GetProductByIdDtoResponse> GetProductById(int id);
        Task<GetAllProductsDtoResponse> GetAllProducts();
        Task<AddProductDtoResponse> AddProduct(AddProductDtoRequest addProductDto);
        Task<UpdateProductDtoResponse> UpdateProduct(int id, UpdateProductDtoRequest updateProductDto);
        Task<DeleteProductDtoResponse> DeleteProduct(int id);
        Task<List<GetProductByIdDtoResponse>> GetFilteredProducts(string? name, decimal? minPrice, decimal? maxPrice, bool? isAvailable, string userRole);

    }
}
