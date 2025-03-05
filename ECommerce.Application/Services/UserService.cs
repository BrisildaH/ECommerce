using AutoMapper;
using Ecommerce.Application.DTO.UserDto;
using Ecommerce.Application.Interface;
using Ecommerce.Application.Resources;
using Ecommerce.Common.Exceptions;
using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IMapper mapper,
            IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }
        public async Task<GetUserByIdDtoResponse> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueUser);
            }
            return _mapper.Map<GetUserByIdDtoResponse>(user);
        }

        public async Task<GetUserByUsernameDtoResponse> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueUser);
            }
            return _mapper.Map<GetUserByUsernameDtoResponse>(user);
        }

        public async Task<GetAllUsersDtoResponse> GetAllUsers()
        {
            var users = await _userRepository.GetAll().ToListAsync();
            return _mapper.Map<GetAllUsersDtoResponse>(users);
        }

        public async Task<AddUserDtoResponse> AddUser(AddUserDtoRequest adduserDto, string userRole)
        {
            if (userRole != "Admin")
            {
                throw new ForbiddenException(StringResourceMessage.ForbiddenAddUserError);
            }

            var existingUser = await _userRepository.GetUserByUsername(adduserDto.Username);
            if (existingUser != null && existingUser.RoleId == adduserDto.RoleId)
            {
                throw new BadRequestException(StringResourceMessage.UserAlreadyExists);
            }

            var user = _mapper.Map<User>(adduserDto);
            user.Password = _passwordHasher.HashPassword(adduserDto.Password);
            await _userRepository.AddAUser(user);

            return _mapper.Map<AddUserDtoResponse>(user);
        }

        public async Task<UpdateUserDtoResponse> UpdateUser(int id, UpdateUserDtoRequest updateUserDto)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueUser);
            }

            var existingUser = await _userRepository.GetUserByUsername(updateUserDto.Username);
            if (existingUser != null && (existingUser.Id != id && existingUser.RoleId == updateUserDto.RoleId))
            {
                throw new BadRequestException(StringResourceMessage.UserAlreadyExists);
            }

            _mapper.Map(updateUserDto, user);
            user.Password = _passwordHasher.HashPassword(updateUserDto.Password);
            await _userRepository.UpdateUser(user);
            return _mapper.Map<UpdateUserDtoResponse>(user);
        }

        public async Task<DeleteUserDtoResponse> DeleteUser(int id, int loggedInUserId)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueUser);
            }

            if (id == loggedInUserId)
            {
                throw new BadRequestException(StringResourceMessage.CannotDeleteOwnAccountError);
            }

            await _userRepository.DeleteUser(id);
            return _mapper.Map<DeleteUserDtoResponse>(user);
        }

        public async Task<UserDtoApp> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
                throw new UnauthorizedAccessException(StringResourceMessage.NotFoundUsermane);

            if (!_passwordHasher.VerifyPassword(password, user.Password))
                throw new UnauthorizedAccessException(StringResourceMessage.NotFoundPassword);

            // Merr vlerat per JWT nga konfigurimi
            var jwtSecretKey = _configuration["Authentication:Jwt:SecretKey"];
            var issuer = _configuration["Authentication:Jwt:Issuer"];
            var audience = _configuration["Authentication:Jwt:Audience"];

            var token = GenerateJwtToken(user, jwtSecretKey, issuer, audience);

            var userDtoApp = _mapper.Map<UserDtoApp>(user);
            userDtoApp.JwtToken = token;

            return userDtoApp;
        }

        // Helper method to generate JWT token
        public string GenerateJwtToken(User user, string jwtSecretKey, string issuer, string audience)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(9),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
