using AutoMapper;
using Ecommerce.Application.DTO.OrderDto;
using Ecommerce.Application.DTO.OrderItemDto;
using Ecommerce.Application.Interface;
using Ecommerce.WebApi.DTO.OrderApiDto;
using Ecommerce.WebApi.DTO.OrderItemApiDto;
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

    public class OrderController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<AddOrderApiRequestDto> _addOrderValidator; 
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IValidator<AddOrderApiRequestDto> addOrderValidator,
            IOrderService orderService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _addOrderValidator = addOrderValidator;
            _orderService = orderService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Admin")]
        // GET api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderByIdApiResponseDto>> GetOrderById([FromRoute] int id)
        {
            var order = await _orderService.GetOrderById(id);

            var getOrderByIdResponseDto = _mapper.Map<GetOrderByIdApiResponseDto>(order);

            return Ok(getOrderByIdResponseDto);

        }

        [Authorize(Roles = "Admin")]
        // GET api/orders
        [HttpGet]
        public async Task<ActionResult<GetAllOrdersApiResponseDto>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            var getAllOrdersDto = _mapper.Map<GetAllOrdersApiResponseDto>(orders);
            return Ok(getAllOrdersDto);
        }

        // POST api/orders
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult<AddOrderApiResponseDto>> AddOrder([FromBody] AddOrderApiRequestDto addOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ValidationResult result = await _addOrderValidator.ValidateAsync(addOrderDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                     .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User not authenticated.");
            }
            var userId = int.Parse(userIdClaim);

            var addOrderDtoRequest = _mapper.Map<AddOrderDtoRequest>(addOrderDto);
            addOrderDtoRequest.UserId = userId;

            var newOrder = await _orderService.AddOrder(addOrderDtoRequest);
            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
        }

        // DELETE api/orders/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("filter")]
        public async Task<ActionResult<GetOrderByIdApiResponseDto>> GetFilteredOrders(
          [FromQuery] int? userId,
          [FromQuery] int? productId,
          [FromQuery] DateTime? startDate,
          [FromQuery] DateTime? endDate,
          [FromQuery] int pageNumber = 1,
          [FromQuery] int pageSize = 10) 
        {
            var orders = await _orderService.GetFilteredOrders(userId, productId, startDate, endDate, pageNumber, pageSize);
            var response = _mapper.Map<List<GetOrderByIdApiResponseDto>>(orders);
            return Ok(response);
        }

        //GET ORDERITEM
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/items")]
        public async Task<ActionResult<OrderItemApiResponseDto>> GetOrderItemsByOrderId([FromRoute] int id)
        {

            var orderItems = await _orderService.GetOrderItemsByOrderId(id);
            var response = _mapper.Map<List<GetOrderItemDtoResponse>>(orderItems);
            return Ok(response);
        }
    }
}
