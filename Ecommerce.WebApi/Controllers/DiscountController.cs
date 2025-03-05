using AutoMapper;
using Ecommerce.Application.DTO.DiscountDto;
using Ecommerce.Application.Interface;
using Ecommerce.WebApi.DTO.DiscountApiDto;
using Ecommerce.WebApi.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IValidator<AddDiscountApiRequestDto> _addDiscountValidator;
        private readonly IValidator<UpdateDiscountApiRequestDto> _updateDiscountValidator;
        private readonly IValidator<AddDiscountsApiRequestDto> _addDiscountsValidator;
        private readonly IValidator<UpdateDiscountsApiRequestDto> _updateDiscountsValidator;
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;


        public DiscountController(
            IValidator<AddDiscountApiRequestDto> addDiscountValidator,
            IValidator<UpdateDiscountApiRequestDto> updateDiscountValidator,
            IDiscountService discountService,
            IMapper mapper,
            IValidator<AddDiscountsApiRequestDto> addDiscountsValidator,
            IValidator<UpdateDiscountsApiRequestDto> updateDiscountsValidator)
        {
            _addDiscountValidator = addDiscountValidator;
            _discountService = discountService;
            _updateDiscountValidator = updateDiscountValidator;
            _mapper = mapper;
            _addDiscountsValidator = addDiscountsValidator;
            _updateDiscountsValidator = updateDiscountsValidator;
        }
        // GET api/discounts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDiscountByIdApiResponseDto>> GetDiscountById([FromRoute] int id)
        {
            var discount = await _discountService.GetDiscountById(id);
            var getDiscountByIdResponseDto = _mapper.Map<GetDiscountByIdApiResponseDto>(discount);
            return Ok(getDiscountByIdResponseDto);
        }

        // GET api/discounts
        [HttpGet]
        public async Task<ActionResult<GetAllDiscountsApiResponseDto>> GetAllDiscounts()
        {
            var discounts = await _discountService.GetAllDiscounts();
            var getAllDiscountsDto = _mapper.Map<GetAllDiscountsApiResponseDto>(discounts);
            return Ok(getAllDiscountsDto);
        }

        // POST api/discounts\
        [HttpPost]
        public async Task<ActionResult<AddDiscountApiResponseDto>> AddDiscount([FromBody] AddDiscountApiRequestDto addDiscountDto)
        {
            if (!ModelState.IsValid)
                ValidationExtensions.CheckModelState(this.ModelState);

            ValidationResult result = await _addDiscountValidator.ValidateAsync(addDiscountDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                ValidationExtensions.CheckModelState(this.ModelState);
            }
            var addDiscountDtoRequest = _mapper.Map<AddDiscountDtoRequest>(addDiscountDto);
            var newDiscount = await _discountService.AddDiscount(addDiscountDtoRequest);

            return CreatedAtAction(nameof(GetDiscountById), new { id = newDiscount.DiscountId }, newDiscount);
        }


        // PUT api/discounts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountApiRequestDto updateDiscountDto)
        {

            if (!ModelState.IsValid)
                ValidationExtensions.CheckModelState(this.ModelState);

            ValidationResult result = await _updateDiscountValidator.ValidateAsync(updateDiscountDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                ValidationExtensions.CheckModelState(this.ModelState);
            }
            var updateDiscountDtoRequest = _mapper.Map<UpdateDiscountDtoRequest>(updateDiscountDto);
            await _discountService.UpdateDiscount(id, updateDiscountDtoRequest);

            return NoContent();
        }

        // DELETE: api/discounts/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDiscount([FromRoute] int id)
        {
            var deleteDiscount = await _discountService.DeleteDiscount(id);
            return NoContent();
        }

        //discount list
        [HttpPost("list")]
        public async Task<ActionResult> AddDiscounts([FromBody] AddDiscountsApiRequestDto addDiscountDto)
        {
            if (!ModelState.IsValid)
                ValidationExtensions.CheckModelState(this.ModelState);

            ValidationResult result = await _addDiscountsValidator.ValidateAsync(addDiscountDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                ValidationExtensions.CheckModelState(this.ModelState);
            }

            var addDiscountDtoRequest = _mapper.Map<AddDiscountsDtoRequest>(addDiscountDto);
            await _discountService.AddDiscounts(addDiscountDtoRequest);

            return NoContent();
        }

        // PUT api/discounts/{id}
        [HttpPut("list")]
        public async Task<ActionResult> UpdateDiscounts([FromBody] UpdateDiscountsApiRequestDto updateDiscountDto)
        {

            if (!ModelState.IsValid)
                ValidationExtensions.CheckModelState(this.ModelState);

            ValidationResult result = await _updateDiscountsValidator.ValidateAsync(updateDiscountDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                ValidationExtensions.CheckModelState(this.ModelState);
            }
            var updateDiscountDtoRequest = _mapper.Map<UpdateDiscountsDtoRequest>(updateDiscountDto);
            await _discountService.UpdateDiscounts(updateDiscountDtoRequest);

            return NoContent();
        }

    }
}