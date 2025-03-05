using AutoMapper;
using Ecommerce.Application.DTO.ProductDto;
using Ecommerce.Application.Interface;
using Ecommerce.Application.Resources;
using Ecommerce.Common.Exceptions;
using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _discountRepository;
        public ProductService(IMapper mapper, IProductRepository productRepository, IDiscountRepository discountRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _discountRepository = discountRepository;
        }

        public async Task<GetProductByIdDtoResponse> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValue);
            }

            return _mapper.Map<GetProductByIdDtoResponse>(product);
        }

        public async Task<GetAllProductsDtoResponse> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts().ToListAsync();
            return _mapper.Map<GetAllProductsDtoResponse>(products);
        }

        public async Task<AddProductDtoResponse> AddProduct(AddProductDtoRequest addProductDto)
        {
            var existingProduct = await _productRepository.GetProductByDescription(addProductDto.Description);

            if (existingProduct != null)
            {
                throw new ConflictException(StringResourceMessage.ConflictValue);

            }

            var product = _mapper.Map<Product>(addProductDto);
            await _productRepository.AddProduct(product);

            return _mapper.Map<AddProductDtoResponse>(product);
        }

        public async Task<UpdateProductDtoResponse> UpdateProduct(int id, UpdateProductDtoRequest updateProductDto)
        {

            var existingProduct = await _productRepository.GetProductById(id);

            if (existingProduct == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValue);
            }
            var existingProductSameDescription = await _productRepository.GetProductByDescription(updateProductDto.Description);
            if (existingProductSameDescription != null && existingProductSameDescription.Id != id)
            {
                throw new ConflictException(StringResourceMessage.ConflictValue);
            }

            existingProduct.Name = updateProductDto.Name;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Stock = updateProductDto.Stock;
            existingProduct.IsAvailable = updateProductDto.IsAvailable;

            await _productRepository.UpdateProduct(existingProduct);
            return _mapper.Map<UpdateProductDtoResponse>(existingProduct);
        }

        public async Task<DeleteProductDtoResponse> DeleteProduct(int id)
        {
            var existingProduct = await _productRepository.GetProductById(id);

            if (existingProduct == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValue);
            }
            if (existingProduct.Stock > 0)
            {
                throw new BadRequestException(StringResourceMessage.BadValue);
            }
            if (existingProduct.IsAvailable)
            {
                throw new BadRequestException(StringResourceMessage.ConflictValue1); ;
            }

            await _productRepository.DeleteProduct(id);
            return _mapper.Map<DeleteProductDtoResponse>(existingProduct);

        }
        public async Task<List<GetProductByIdDtoResponse>> GetFilteredProducts(string? name, decimal? minPrice, decimal? maxPrice, bool? isAvailable, string userRole)
        {
            bool isAdmin;
            if (userRole == "Admin")
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
            var products = await _productRepository.GetFilteredProducts(name, minPrice, maxPrice, isAvailable, isAdmin);
            return _mapper.Map<List<GetProductByIdDtoResponse>>(products);
        }

    }
}