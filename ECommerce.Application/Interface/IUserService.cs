using Ecommerce.Application.DTO.UserDto;
using Ecommerce.Domain;
using Ecommerce.WebApi.DTO.UserApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interface
{
    public interface IUserService
    {
        Task<GetUserByIdDtoResponse> GetUserById(int id);
        Task<GetUserByUsernameDtoResponse> GetUserByUsername(string username);
        Task<GetAllUsersDtoResponse> GetAllUsers();
        Task<AddUserDtoResponse> AddUser(AddUserDtoRequest adduserDto, string userRole);
        Task<UpdateUserDtoResponse> UpdateUser(int id, UpdateUserDtoRequest updateUserDto);
        Task<DeleteUserDtoResponse> DeleteUser(int id, int loggedInUserId);
        Task<UserDtoApp> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user, string jwtSecretKey, string issuer, string audience);
    }
}
