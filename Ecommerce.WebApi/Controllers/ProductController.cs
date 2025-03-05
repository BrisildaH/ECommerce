using AutoMapper;
using Ecommerce.Application.DTO.ProductDto;
using Ecommerce.Application.Interface;
using Ecommerce.WebApi.DTO.ProductApiDto;
using Ecommerce.WebApi.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IValidator<AddProductApiRequestDto> _addProductValidator;
        private readonly IValidator<UpdateProductApiRequestDto> _updateProductValidator;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IValidator<AddProductApiRequestDto> addProductValidator,
            IValidator<UpdateProductApiRequestDto> updateProductValidator,
            IProductService productService,
            IMapper mapper)
        {
            _addProductValidator = addProductValidator;
            _productService = productService;
            _updateProductValidator = updateProductValidator;
            _mapper = mapper;
        }

        // GET api/products/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductByIdApiResponseDto>> GetProductById([FromRoute] int id)
        {
            var product = await _productService.GetProductById(id);
            var getProductByIdResponseDto = _mapper.Map<GetProductByIdApiResponseDto>(product);
            return Ok(getProductByIdResponseDto);
        }

        // GET api/products
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<GetAllProductsApiResponseDto>> GetAllProducts()
        {

            var products = await _productService.GetAllProducts();
            var getAllProductsDto = _mapper.Map<GetAllProductsApiResponseDto>(products);
            return Ok(getAllProductsDto);
        }

        // POST api/products
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<AddProductApiResponseDto>> AddProduct([FromBody] AddProductApiRequestDto addProductDto)
        {
            if (!ModelState.IsValid)
                ValidationExtensions.CheckModelState(this.ModelState);

            ValidationResult result = await _addProductValidator.ValidateAsync(addProductDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                ValidationExtensions.CheckModelState(this.ModelState);
            }
            var addProductDtoRequest = _mapper.Map<AddProductDtoRequest>(addProductDto);
            var newProduct = await _productService.AddProduct(addProductDtoRequest);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ProductId }, newProduct);
        }


        // PUT api/products/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductApiRequestDto updateProductDto)
        {

            if (!ModelState.IsValid)
                ValidationExtensions.CheckModelState(this.ModelState);

            ValidationResult result = await _updateProductValidator.ValidateAsync(updateProductDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                ValidationExtensions.CheckModelState(this.ModelState);
            }
            var updateProductDtoRequest = _mapper.Map<UpdateProductDtoRequest>(updateProductDto);
            await _productService.UpdateProduct(id, updateProductDtoRequest);

            return NoContent();
        }

        // DELETE: api/products/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var deleteProduct = await _productService.DeleteProduct(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("search")]
        public async Task<ActionResult<List<GetProductByIdApiResponseDto>>> SearchProducts(
       [FromQuery] string? name,
       [FromQuery] decimal? minPrice,
       [FromQuery] decimal? maxPrice,
       [FromQuery] bool? isAvailable)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var products = await _productService.GetFilteredProducts(name, minPrice, maxPrice, isAvailable, userRole);
            var response = _mapper.Map<List<GetProductByIdApiResponseDto>>(products);
            return Ok(response);
        }
    }
}
