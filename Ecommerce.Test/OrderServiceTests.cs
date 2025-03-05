//using AutoMapper;
//using Ecommerce.Application.DTO.OrderDto;
//using Ecommerce.Application.DTO.OrderItemDto;
//using Ecommerce.Application.DTO.ProductDto;
//using Ecommerce.Application.Resources;
//using Ecommerce.Application.Services;
//using Ecommerce.Common.Exceptions;
//using Ecommerce.Domain;
//using Ecommerce.Domain.Interface;
//using Ecommerce.Infrastructure.Migrations;
//using Ecommerce.WebApi.DTO.OrderApiDto;
//using Microsoft.AspNetCore.Http;
//using NSubstitute;

//namespace Ecommerce.Test
//{
//    [TestClass]
//    public class OrderServiceTests
//    {
//        private IOrderRepository _orderRepository;
//        private IProductRepository _productRepository;
//        private IMapper _mapper;
//        private IUserRepository _userRepository;
//        private IDiscountRepository _discountRepository;
//        private IOrderItemRepository _orderItemRepository;
//        private IHttpContextAccessor _httpContextAccessor;
//        private OrderService _orderService;

//        [TestInitialize]
//        public void Setup()
//        {
//            _orderRepository = Substitute.For<IOrderRepository>();
//            _mapper = Substitute.For<IMapper>();
//            _productRepository = Substitute.For<IProductRepository>();
//            _discountRepository = Substitute.For<IDiscountRepository>();
//            _orderItemRepository = Substitute.For<IOrderItemRepository>();
//            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();


//            _orderService = new OrderService(
//                _orderRepository, _mapper, _productRepository,
//                _discountRepository, _orderItemRepository, _httpContextAccessor);
//        }

//        [TestMethod]
//        public async Task GetOrderById_ShouldThrowNotFoundException_WhenOrderNotFound()
//        {
//            int id = 15;
//            _orderRepository.GetOrderById(id).Returns(Task.FromResult<Order>(null));
//            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _orderService.GetOrderById(id));

//        }

//        [TestMethod]
//        public async Task GetOrderById_ShouldReturnOrder_WhenOrderExists()
//        {
//            int orderId = 15;
//            var order = new Order { Id = orderId, UserId = 2 };
//            var orderDto = new GetOrderByIdDtoResponse { UserId = 2 };

//            _orderRepository.GetOrderById(orderId).Returns(Task.FromResult(order));
//            _mapper.Map<GetOrderByIdDtoResponse>(order).Returns(orderDto);

//            var result = await _orderService.GetOrderById(orderId);

//            Assert.AreEqual(orderDto.UserId, result.UserId);
//        }

//        [TestMethod]

//        public async Task GetAllOrders_ShouldReturnAllOrders_WhenOrdersExist()
//        {
//            var orders = new List<Order>()
//                {
//        new Order { Id = 1, TotalAmount = 100, UserId = 1 },
//        new Order { Id = 2, TotalAmount = 200, UserId = 2}
//    };
//            var ordersDto = new List<GetOrderByIdDtoResponse>
//    {
//        new GetOrderByIdDtoResponse {  TotalAmount = 100, UserId = 1 },
//        new GetOrderByIdDtoResponse { TotalAmount = 200, UserId = 2 }
//    };
//            _orderRepository.GetAll().Returns(orders.AsQueryable());

//            _mapper.Map<List<GetOrderByIdDtoResponse>>(orders).Returns(ordersDto);

//            var result = await _orderService.GetAllOrders();

//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Orders.Count);
//        }

//        [TestMethod]
//        public async Task GetAllProducts_ShouldReturnEmptyList_WhenNoProductsExist()
//        {
//            var orders = new List<Order>();
//            var ordersDto = new List<GetOrderByIdDtoResponse>();

//            _orderRepository.GetAll().Returns(orders.AsQueryable());

//            _mapper.Map<List<GetOrderByIdDtoResponse>>(orders).Returns(ordersDto);

//            var result = await _orderService.GetAllOrders();

//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Orders.Count);
//        }

//        [TestMethod]
//        public async Task AddOrder_ShouldThrowBadRequestException_WhenOrderItemsAreNull()
//        {
//            var addOrderDto = new AddOrderDtoRequest { OrderItems = null };

//            var exception = await Assert.ThrowsExceptionAsync<BadRequestException>(() => _orderService.AddOrder(addOrderDto));

//            Assert.AreEqual(StringResourceMessage.OrderProductEmpty, exception.Message);
//        }

//        [TestMethod]
//        public async Task AddOrder_ShouldThrowNotFoundException_WhenProductDoesNotExist()
//        {
//            var productId = 1;
//            var addOrderDto = new AddOrderDtoRequest
//            {
//                UserId = 1,
//                OrderItems = new List<OrderItemRequest>
//               {
//                 new OrderItemRequest { ProductId = 1, Quantity = 2 }
//                 }
//            };
//            _productRepository.GetProductById(productId).Returns((Product)null);

//            var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _orderService.AddOrder(addOrderDto));

//            Assert.AreEqual(StringResourceMessage.NotFoundValue, exception.Message);
//        }

//        [TestMethod]
//        public async Task AddOrder_ShouldThrowBadRequestException_WhenQuantityExceedsStock()
//        {
//            var productId = 1;
//            var addOrderDto = new AddOrderDtoRequest
//            {
//                OrderItems = new List<OrderItemRequest>
//            {
//                new OrderItemRequest { ProductId = 1, Quantity = 10 }
//            }
//            };

//            var product = new Product { Id = 1, Stock = 5 };
//            _productRepository.GetProductById(productId).Returns(product);

//            var exception = await Assert.ThrowsExceptionAsync<BadRequestException>(() => _orderService.AddOrder(addOrderDto));

//            Assert.AreEqual(StringResourceMessage.InsufficientStockProduct, exception.Message);
//        }

//        [TestMethod]
//        public async Task AddOrder_ShouldAddOrderSuccessfully()
//        {
//            var productId = 1;
//            var addOrderDto = new AddOrderDtoRequest
//            {
//                UserId = 1,
//                OrderItems = new List<OrderItemRequest>
//        {
//            new OrderItemRequest { ProductId = 1, Quantity = 2 }
//        }
//            };

//            var product = new Product { Id = 1, Stock = 10, Price = 100 };
//            _productRepository.GetProductById(productId).Returns(product);

//            var order = new Order { Id = 1, UserId = addOrderDto.UserId }; 
//            _orderRepository.AddOrder(Arg.Any<Order>())
//                .Returns(Task.CompletedTask); 

//            var addOrderDtoResponse = new AddOrderDtoResponse { Id = 1 };
//            _mapper.Map<AddOrderDtoResponse>(Arg.Any<Order>()).Returns(addOrderDtoResponse);

//            var result = await _orderService.AddOrder(addOrderDto);

//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Id); 
//        }


//        [TestMethod]
//        public async Task DeleteOrder_ShouldThrowNotFoundException_WhenOrderDoesNotExist()
//        {
//            var orderId = 1;
//            _orderRepository.GetOrderById(orderId).Returns((Order)null);

//            var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _orderService.DeleteOrder(1));

//            Assert.AreEqual(StringResourceMessage.NotFoundValueO, exception.Message);
//        }

//        [TestMethod]
//        public async Task DeleteOrder_ShouldDeleteOrderSuccessfully()
//        {
//            var orderId = 1;
//            var order = new Order { Id = 1 };
//            _orderRepository.GetOrderById(orderId).Returns(order);

//            var result = await _orderService.DeleteOrder(1);

//            await _orderRepository.Received(1).DeleteOrder(1);
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public async Task GetFilteredOrders_ShouldReturnOrders_WhenParametersAreValid()
//        {
//            var orders = new List<Order>
//    {
//        new Order { Id = 1, UserId = 1, TotalAmount = 100 },
//        new Order { Id = 2, UserId = 1, TotalAmount = 150 }
//    };

//            _orderRepository.GetFilteredOrders(Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<int>(), Arg.Any<int>())
//                .Returns(orders);

//            var expectedDto = new List<GetOrderByIdDtoResponse>
//    {
//        new GetOrderByIdDtoResponse { UserId = 1, TotalAmount = 100 },
//        new GetOrderByIdDtoResponse { UserId = 1, TotalAmount = 150 }
//    };

//            _mapper.Map<List<GetOrderByIdDtoResponse>>(orders).Returns(expectedDto);

//            var result = await _orderService.GetFilteredOrders(1, null, null, null, 1, 10);

//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//        }

//        [TestMethod]
//        public async Task GetOrderItemsByOrderId_ShouldThrowNotFoundException_WhenOrderDoesNotExist()
//        {
//            var orderId = 1;
//            _orderRepository.GetOrderById(orderId).Returns((Order)null);

//            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
//            {
//                await _orderService.GetOrderItemsByOrderId(1);
//            });
//        }
//        [TestMethod]
//        public async Task GetOrderItemsByOrderId_ShouldReturnOrderItems_WhenOrderExists()
//        {
//            var orderId = 1;
//            var order = new Order { Id = 1, UserId = 1 };
//            var orderItems = new List<OrderItem>
//    {
//        new OrderItem { Id = 1, ProductId = 1, Quantity = 2, Price = 50 },
//        new OrderItem { Id = 2, ProductId = 2, Quantity = 1, Price = 100 }
//    };

//            var expectedDto = new List<GetOrderItemDtoResponse>
//    {
//        new GetOrderItemDtoResponse { ProductId = 1, Quantity = 2, Price = 50 },
//        new GetOrderItemDtoResponse { ProductId = 2, Quantity = 1, Price = 100 }
//    };

//            _orderRepository.GetOrderById(orderId).Returns(order);
//            _orderItemRepository.GetOrderItemsByOrderId(orderId).Returns(orderItems);
//            _mapper.Map<List<GetOrderItemDtoResponse>>(orderItems).Returns(expectedDto);

//            var result = await _orderService.GetOrderItemsByOrderId(1);

//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//        }
//    }
//}