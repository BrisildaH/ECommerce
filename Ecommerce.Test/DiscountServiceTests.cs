using AutoMapper;
using Ecommerce.Application.DTO.DiscountDto;
using Ecommerce.Application.DTO.ProductDto;
using Ecommerce.Application.Interface;
using Ecommerce.Application.Services;
using Ecommerce.Common.Exceptions;
using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using NSubstitute;

namespace Ecommerce.Test
{
    [TestClass]
    public class DiscountServiceTests
    {
        private IDiscountRepository _discountRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private DiscountService _discountService;

        [TestInitialize]
        public void Setup()
        {
            _discountRepository = Substitute.For<IDiscountRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();

            _discountService = new DiscountService(_discountRepository, _mapper, _productRepository, _userRepository);
        }
        public async Task GetDiscountById_ShouldThrowNotFoundException_WhenDiscountNotFound()
        {
            int id = 999;
            _discountRepository.GetDiscountById(id).Returns(Task.FromResult<Discount>(null));

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _discountService.GetDiscountById(id));
        }

        [TestMethod]
        public async Task GetDiscountById_ShouldReturnDiscount_WhenDiscountExists()
        {
            int discountId = 15;
            var discount = new Discount { Id = discountId, Description = "Summer 2025" };
            var discountDto = new GetDiscountByIdDtoResponse { Description = "Summer 2025" };

            _discountRepository.GetDiscountById(discountId).Returns(Task.FromResult(discount));
            _mapper.Map<GetDiscountByIdDtoResponse>(discount).Returns(discountDto);

            var result = await _discountService.GetDiscountById(discountId);

            Assert.AreEqual(discountDto.Description, result.Description);
        }

        [TestMethod]

        public async Task GetAllDiscounts_ShouldReturnAllDiscounts_WhenDiscountsExist()
        {
            var discounts = new List<Discount>()
                {
        new Discount { Id = 1, Description = "Discount 1",  Percentage = 10, UserId = 2, ProductId = 3 },
        new Discount { Id = 2, Description = "Discount 2", Percentage = 20, UserId = 20, ProductId = 4 }
    };
            var discountsDto = new List<GetDiscountByIdDtoResponse> // ndoshta duhet GetAllDiscountsDtoResponse
    {
        new GetDiscountByIdDtoResponse {  Description = "Description 1",  Percentage = 10, UserId = 2, ProductId = 3  },
        new GetDiscountByIdDtoResponse { Description = "Description 2", Percentage = 20, UserId = 20, ProductId = 4 }
    };
            _discountRepository.GetAllDiscounts().Returns(discounts.AsQueryable());

            _mapper.Map<List<GetDiscountByIdDtoResponse>>(discounts).Returns(discountsDto);

            var result = await _discountService.GetAllDiscounts();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Discounts.Count);
            Assert.AreEqual("Discount 1", result.Discounts[0].Description);
            Assert.AreEqual("Discount 2", result.Discounts[1].Description);

        }

        [TestMethod]
        public async Task GetAllDiscounts_ShouldReturnEmptyList_WhenNoDiscountsExist()
        {
            var discounts = new List<Discount>();
            var discountsDto = new List<GetDiscountByIdDtoResponse>();

            _discountRepository.GetAllDiscounts().Returns(discounts.AsQueryable());

            _mapper.Map<List<GetDiscountByIdDtoResponse>>(discounts).Returns(discountsDto);

            var result = await _discountService.GetAllDiscounts();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Discounts.Count);
        }

        [TestMethod]
        public async Task AddDiscount_ShouldThrowConfictException_WhenDiscountAlreadyExists()
        {
            var addDiscountDto = new AddDiscountDtoRequest
            {
                Description = "test",
                Percentage = 10,
                UserId = 1,
                ProductId = 1
            };
            var existingDiscount = new Discount { UserId = 1, ProductId = 1 };
            _discountRepository.GetDiscountByProductUserId(addDiscountDto.UserId, addDiscountDto.ProductId).Returns(existingDiscount);
            await Assert.ThrowsExceptionAsync<ConflictException>(async () =>
                                           await _discountService.AddDiscount(addDiscountDto));
        }

        [TestMethod]
        public async Task AddDiscount_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            var addDiscountDto = new AddDiscountDtoRequest
            {
                ProductId = 1,
                UserId = 1
            };
            _discountRepository.GetDiscountByProductUserId(addDiscountDto.ProductId, addDiscountDto.UserId)
                                                         .Returns((Discount)null);
            _productRepository.GetProductById(addDiscountDto.ProductId).Returns((Product)null);

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
                      await _discountService.AddDiscount(addDiscountDto));
        }

        [TestMethod]
        public async Task AddDiscount_ShouldThrowNotFoundExeption_WhenUserDoesNotExist()
        {
            var addDiscountDto = new AddDiscountDtoRequest
            {
                ProductId = 1,
                UserId = 1
            };
            _discountRepository.GetDiscountByProductUserId(addDiscountDto.ProductId, addDiscountDto.UserId).Returns((Discount)null);
            _productRepository.GetProductById(addDiscountDto.ProductId).Returns(new Product { Id = addDiscountDto.ProductId });
            _userRepository.GetUserById(addDiscountDto.UserId).Returns((User)null);

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
             await _discountService.AddDiscount(addDiscountDto));
        }

        [TestMethod]
        public async Task AddDiscount_ShouldAddDiscountSuccessfully()
        {
            var addDiscountDto = new AddDiscountDtoRequest
            {
                ProductId = 1,
                UserId = 1,
                Percentage = 10
            };

            var product = new Product { Id = 1, Price = 100 }; 
            var user = new User { Id = 1 };  
            var existingDiscount = (Discount)null;  

            _discountRepository.GetDiscountByProductUserId(addDiscountDto.ProductId, addDiscountDto.UserId)
                .Returns(existingDiscount); 
            _productRepository.GetProductById(addDiscountDto.ProductId).Returns(product);  
            _userRepository.GetUserById(addDiscountDto.UserId).Returns(user);

            var discount = new Discount { Id = 1, ProductId = addDiscountDto.ProductId, UserId = addDiscountDto.UserId, Percentage = addDiscountDto.Percentage };
            var addDiscountDtoResponse = new AddDiscountDtoResponse { DiscountId = discount.Id };
            _mapper.Map<Discount>(addDiscountDto).Returns(discount); 
            _mapper.Map<AddDiscountDtoResponse>(discount).Returns(addDiscountDtoResponse);  

            var result = await _discountService.AddDiscount(addDiscountDto);

            Assert.IsNotNull(result);  
            Assert.AreEqual(1, result.DiscountId); 
        }


        [TestMethod]
        public async Task UpdateDiscount_ShouldReturnUpdatedDiscount_WhenDiscountIsValid()
        {
            var discountId = 1;
            var updateDiscountDto = new UpdateDiscountDtoRequest
            {
                Description = "Updated Discount",
                Percentage = 20,
                ProductId = 1,
                UserId = 1
            };

            var existingDiscount = new Discount
            {
                Id = 1,
                Description = "Old Discount",
                Percentage = 10,
                ProductId = 1,
                UserId = 1
            };

            var updatedDiscountDto = new UpdateDiscountDtoResponse { };

            _discountRepository.GetDiscountById(discountId).Returns(existingDiscount);
            _productRepository.GetProductById(updateDiscountDto.ProductId).Returns(new Product { Id = updateDiscountDto.ProductId });
            _userRepository.GetUserById(updateDiscountDto.UserId).Returns(new User { Id = updateDiscountDto.UserId });

            _mapper.Map<UpdateDiscountDtoResponse>(existingDiscount).Returns(updatedDiscountDto);

            var result = await _discountService.UpdateDiscount(discountId, updateDiscountDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(updatedDiscountDto, result);
        }

        [TestMethod]
        public async Task UpdateDiscount_ShouldThrowNotFoundException_WhenDiscountDoesNotExist()
        {
            var discountId = 1;
            var updateDiscountDto = new UpdateDiscountDtoRequest { Description = "Test" };

            _discountRepository.GetDiscountById(discountId).Returns((Discount)null);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _discountService.UpdateDiscount(discountId, updateDiscountDto));
        }

        [TestMethod]
        public async Task UpdateDiscount_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            var discountId = 1;
            var updateDiscountDto = new UpdateDiscountDtoRequest { Id = 1, Description = "Test" };
            var existingDiscount = new Discount { Id = 1, Description = "Test1" };

            _productRepository.GetProductById(updateDiscountDto.ProductId).Returns((Product)null);
            _userRepository.GetUserById(updateDiscountDto.UserId).Returns(new User { Id = updateDiscountDto.UserId });

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _discountService.UpdateDiscount(discountId, updateDiscountDto));
        }

        [TestMethod]
        public async Task UpdateDiscount_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            var discountId = 1;
            var updateDiscountDto = new UpdateDiscountDtoRequest { Description = "Test" };
            var existingDiscount = new Discount { Id = 1, Description = "Test1" };

            _discountRepository.GetDiscountByDescription(updateDiscountDto.Description).Returns((Discount)null);
            _productRepository.GetProductById(updateDiscountDto.ProductId).Returns(new Product { Id = updateDiscountDto.ProductId });
            _userRepository.GetUserById(updateDiscountDto.UserId).Returns((User)null);

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
           await _discountService.UpdateDiscount(updateDiscountDto.Id, updateDiscountDto));
        }

        [TestMethod]
        public async Task UpdateDiscount_ShouldThrowConflictException_WhenDiscountForSameProductAndUserExists()
        {
            var discountId = 2;
            var updateDiscountDto = new UpdateDiscountDtoRequest
            {
                Description = "Test",
                ProductId = 1,
                UserId = 1
            };

            var existingDiscount = new Discount
            {
                Id = 1,
                Description = "Test1",
                ProductId = 1,
                UserId = 1
            };
            _discountRepository.GetDiscountById(discountId).Returns(existingDiscount);
            _discountRepository.GetDiscountByProductUserId(updateDiscountDto.UserId, updateDiscountDto.ProductId)
          .Returns(existingDiscount);

            _productRepository.GetProductById(updateDiscountDto.ProductId).Returns(new Product { Id = updateDiscountDto.ProductId });
            _userRepository.GetUserById(updateDiscountDto.UserId).Returns(new User { Id = updateDiscountDto.UserId });

            await Assert.ThrowsExceptionAsync<ConflictException>(async () =>
            await _discountService.UpdateDiscount(discountId, updateDiscountDto));
        }

        [TestMethod]
        public async Task DeleteDiscount_ShouldThrowNotFoundException_WhenDiscountDoesNotExist()
        {
            var discountId = 1;
            _discountRepository.GetDiscountById(discountId).Returns((Discount)null);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _discountService.DeleteDiscount(discountId));
        }

        [TestMethod]
        public async Task DeleteDiscount_ShouldReturnDeletedDiscount_WhenDiscountDeletedSuccessfully()
        {
            var discountId = 1;
            var existingDiscount = new Discount { Id = discountId, Description = "test", Percentage = 10, ProductId = 1, UserId = 1 };
            var deletedDiscountDto = new DeleteDiscountDtoResponse { };


            _discountRepository.GetDiscountById(discountId).Returns(existingDiscount);
            _mapper.Map<DeleteDiscountDtoResponse>(existingDiscount).Returns(deletedDiscountDto);

            var result = await _discountService.DeleteDiscount(discountId);

            Assert.AreEqual(deletedDiscountDto, result);
            await _discountRepository.Received(1).DeleteDiscount(discountId);
        }
    }
}