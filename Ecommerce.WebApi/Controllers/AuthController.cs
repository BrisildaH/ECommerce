using AutoMapper;
using Ecommerce.Application.Interface;
using Ecommerce.WebApi.DTO.UserApiDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController (IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }   

        [HttpPost("login")]
        public async Task<ActionResult<UserApiDto>> Login([FromBody] LoginDto loginDto)
        {
            var userDto = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            var userApiDto = _mapper.Map<UserApiDto>(userDto);
            return Ok(userApiDto);
        }
    }
}
