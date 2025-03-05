using AutoMapper;
using Ecommerce.Application.DTO.ProductDto;
using Ecommerce.Application.Services;
using Ecommerce.Common.Exceptions;
using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using NSubstitute;


namespace Ecommerce.Test
{
    [TestClass]
    public class ProductServiceTests
    {
        private IProductRepository _productRepository;
        private IDiscountRepository _discountRepository;
        private IMapper _mapper;
        private ProductService _productService;

        [TestInitialize]
        public void Setup()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _discountRepository = Substitute.For<IDiscountRepository>();

            _productService = new ProductService(_mapper, _productRepository, _discountRepository);
        }

        [TestMethod]
        public async Task GetProductById_ShouldThrowNotFoundException_WhenProductNotFound()
        {
            int id = 15;
            _productRepository.GetProductById(id).Returns(Task.FromResult<Product>(null));

            // _productRepository.GetProductById(id).Returns(new Product());

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _productService.GetProductById(id));
        }

        [TestMethod]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            int productId = 15;
            var product = new Product { Id = productId, Description = "Samsung S24" };
            var productDto = new GetProductByIdDtoResponse { Description = "Samsung S24" };

            _productRepository.GetProductById(productId).Returns(Task.FromResult(product));
            _mapper.Map<GetProductByIdDtoResponse>(product).Returns(productDto);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.AreEqual(productDto.Description, result.Description);
        }

        [TestMethod]

        public async Task GetAllProducts_ShouldReturnAllProducts_WhenProductsExist()
        {
            var products = new List<Product>()
                {
        new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 100, Stock = 10, IsAvailable = true },
        new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 200, Stock = 20, IsAvailable = true }
    };
            var productsDto = new List<GetProductByIdDtoResponse>
    {
        new GetProductByIdDtoResponse {  Name = "Product 1", Description = "Description 1", Price = 100, Stock = 10, IsAvailable = true },
        new GetProductByIdDtoResponse {  Name = "Product 2", Description = "Description 2", Price = 200, Stock = 20, IsAvailable = true }
    };
            _productRepository.GetAllProducts().Returns(products.AsQueryable());

            _mapper.Map<List<GetProductByIdDtoResponse>>(products).Returns(productsDto);

            var result = await _productService.GetAllProducts();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Products.Count);
            Assert.AreEqual("Product 1", result.Products[0].Name);
            Assert.AreEqual("Product 2", result.Products[1].Name);

        }

        [TestMethod]
        public async Task GetAllProducts_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            var products = new List<Product>();
            var productsDto = new List<GetProductByIdDtoResponse>();

            _productRepository.GetAllProducts().Returns(products.AsQueryable());

            _mapper.Map<List<GetProductByIdDtoResponse>>(products).Returns(productsDto);

            var result = await _productService.GetAllProducts();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Products.Count);
        }


        [TestMethod]
        public async Task AddProduct_ShouldThrowConflictException_WhenProductAlreadyExists()
        {
            var addProductDto = new AddProductDtoRequest
            {
                Description = "Samsung S24"
            };

            var existingProduct = new Product { Id = 1, Description = "Samsung S24" };
            _productRepository.GetProductByDescription(addProductDto.Description).Returns(Task.FromResult(existingProduct));

            await Assert.ThrowsExceptionAsync<ConflictException>(async () =>
                await _productService.AddProduct(addProductDto));
        }

        [TestMethod]
        public async Task AddProduct_ShouldReturnProduct_WhenProductAddedSuccessfully()
        {
            var addProductDto = new AddProductDtoRequest { Description = "Iphone" };
            var product = new Product { Id = 1, Description = "Iphone" };
            var addProductDtoResponse = new AddProductDtoResponse { ProductId = 1, Description = "Iphone" };

            _productRepository.GetProductByDescription(addProductDto.Description).Returns(Task.FromResult<Product>(null));
            _mapper.Map<Product>(addProductDto).Returns(product);
            _mapper.Map<AddProductDtoResponse>(product).Returns(addProductDtoResponse);

            var result = await _productService.AddProduct(addProductDto);

            Assert.AreEqual(addProductDtoResponse.Description, result.Description);
        }

        [TestMethod]
        public async Task UpdateProduct_ShouldReturnUpdatedProduct_WhenProductIsValid()
        {
            var productId = 1;
            var updateProductDto = new UpdateProductDtoRequest { Description = "Iphone16" };

            var existingProduct = new Product { Description = "Iphone15" };
            var updatedProductDto = new UpdateProductDtoResponse { Description = "Iphone16" };

            _productRepository.GetProductById(productId).Returns(existingProduct);
            _productRepository.GetProductByDescription(updateProductDto.Description).Returns((Product)null);
            _mapper.Map<UpdateProductDtoResponse>(existingProduct).Returns(updatedProductDto);

            var result = await _productService.UpdateProduct(productId, updateProductDto);

            Assert.AreEqual(updatedProductDto.Description, result.Description);
        }

        [TestMethod]
        public async Task UpdateProduct_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            var productId = 1;
            var updateProductDto = new UpdateProductDtoRequest { Description = "Iphone16" };

            _productRepository.GetProductById(productId).Returns((Product)null);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _productService.UpdateProduct(productId, updateProductDto));
        }

        [TestMethod]
        public async Task UpdateProduct_ShouldThrowConflictException_WhenProductWithSameDescriptionExists()
        {
            var productId = 1;
            var updateProductDto = new UpdateProductDtoRequest { Description = "Iphone16" };
            var existingProduct = new Product { Id = 1, Description = "Iphone15" };
            var existingProductSameDescription = new Product { Id = 2, Description = "Iphone15" };

            _productRepository.GetProductById(productId).Returns(existingProduct);
            _productRepository.GetProductByDescription(updateProductDto.Description).Returns(existingProductSameDescription);

            await Assert.ThrowsExceptionAsync<ConflictException>(() => _productService.UpdateProduct(productId, updateProductDto));
        }

        [TestMethod]
        public async Task DeleteProduct_ShouldReturnDeletedProduct_WhenProductDeletedSuccessfully()
        {
            var productId = 1;
            var existingProduct = new Product { Id = productId, Stock = 0, IsAvailable = false };
            var deletedProductDto = new DeleteProductDtoResponse { };


            _productRepository.GetProductById(productId).Returns(existingProduct);
            _mapper.Map<DeleteProductDtoResponse>(existingProduct).Returns(deletedProductDto);

            var result = await _productService.DeleteProduct(productId);

            Assert.AreEqual(deletedProductDto, result);
            await _productRepository.Received(1).DeleteProduct(productId);
        }

        [TestMethod]
        public async Task DeleteProduct_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            var productId = 1;
            _productRepository.GetProductById(productId).Returns((Product)null);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _productService.DeleteProduct(productId));
        }

        [TestMethod]
        public async Task DeleteProduct_ShouldThrowBadRequestException_WhenProductHasStock()
        {
            var productId = 1;
            var existingProduct = new Product { Id = productId, Stock = 10, IsAvailable = false };

            _productRepository.GetProductById(productId).Returns(existingProduct);

            await Assert.ThrowsExceptionAsync<BadRequestException>(() => _productService.DeleteProduct(productId));
        }

        [TestMethod]
        public async Task DeleteProduct_ShouldThrowBadRequestException_WhenProductIsAvailable()
        {
            var productId = 1;
            var existingProduct = new Product { Id = productId, Stock = 0, IsAvailable = true };

            _productRepository.GetProductById(productId).Returns(existingProduct);

            await Assert.ThrowsExceptionAsync<BadRequestException>(() => _productService.DeleteProduct(productId));
        }

        [TestMethod]
        public async Task GetFilteredProducts_ShouldReturnEmptyList_WhenNoProductsMatchFilters()
        {
            var products = new List<Product>();
            var filteredProductsDto = new List<GetProductByIdDtoResponse>();

            _productRepository.GetFilteredProducts("NonExistingProduct", 100, 200, true, false).Returns(products);
            _mapper.Map<List<GetProductByIdDtoResponse>>(products).Returns(filteredProductsDto);

            var result = await _productService.GetFilteredProducts("NonExistingProduct", 100, 200, true, "User");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetFilteredProducts_ShouldReturnFilteredProducts_WhenFiltersAreApplied()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, IsAvailable = true },
                new Product { Id = 2, Name = "Product 2", Price = 20, IsAvailable = false }
            };
            var filteredProductsDto = new List<GetProductByIdDtoResponse>
            {
                new GetProductByIdDtoResponse { Name = "Product 1", Price = 10, IsAvailable = true }
            };

            _productRepository.GetFilteredProducts("Product 1", 5, 15, true, false).Returns(products);
            _mapper.Map<List<GetProductByIdDtoResponse>>(products).Returns(filteredProductsDto);
            var result = await _productService.GetFilteredProducts("Product 1", 5, 15, true, "User");

            Assert.IsNotNull(result);
            Assert.AreEqual("Product 1", result[0].Name);
        }
    }
}
