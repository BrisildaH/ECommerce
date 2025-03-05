using AutoMapper;
using Ecommerce.Application.DTO.ProductDto;
using Ecommerce.Application.DTO.UserDto;
using Ecommerce.Application.Interface;
using Ecommerce.Application.Services;
using Ecommerce.WebApi.DTO.ProductApiDto;
using Ecommerce.WebApi.DTO.UserApiDto;
using Ecommerce.WebApi.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
  
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IValidator<AddUserApiRequestDto> _addUserValidator;
        private readonly IValidator<UpdateUserApiRequestDto> _updateUserValidator;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService,
                              IMapper mapper,
                              IValidator<AddUserApiRequestDto> addUserValidator,
                              IValidator<UpdateUserApiRequestDto> updateUserValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _addUserValidator = addUserValidator;
            _updateUserValidator = updateUserValidator;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<GetAllUsersApiResponseDto>> GetAll()
        {
            var users = await _userService.GetAllUsers();
            var getAllUsersDto = _mapper.Map<GetAllUsersApiResponseDto>(users);
            return Ok(getAllUsersDto);
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserByIdApiResponseDto>> GetUserById([FromRoute] int id)
        {
            var user = await _userService.GetUserById(id);
            var getUserByIdResponseDto = _mapper.Map<GetUserByIdApiResponseDto>(user);
            return Ok(getUserByIdResponseDto);
        }

        [HttpPost]
        public async Task<ActionResult<AddUserApiResponseDto>> AddUser([FromBody] AddUserApiRequestDto addUserDto)
        {

            if (!ModelState.IsValid)
                ValidationExtensions.CheckModelState(this.ModelState);

            ValidationResult result = await _addUserValidator.ValidateAsync(addUserDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                ValidationExtensions.CheckModelState(this.ModelState);
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var addUserDtoRequest = _mapper.Map<AddUserDtoRequest>(addUserDto);
            var newUser = await _userService.AddUser(addUserDtoRequest, userRole);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // PUT api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserApiRequestDto updateUserDto)
        {

            if (!ModelState.IsValid)
                ValidationExtensions.CheckModelState(this.ModelState);

            ValidationResult result = await _updateUserValidator.ValidateAsync(updateUserDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                ValidationExtensions.CheckModelState(this.ModelState);
            }
            var updateUserDtoRequest = _mapper.Map<UpdateUserDtoRequest>(updateUserDto);
            await _userService.UpdateUser(id, updateUserDtoRequest);

            return NoContent();
        }
        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _userService.DeleteUser(id, loggedInUserId);

            return NoContent();
        }
    }
}
