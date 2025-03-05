using AutoMapper;
using Ecommerce.Application.DTO.OrderDto;
using Ecommerce.Application.DTO.UserDto;
using Ecommerce.WebApi.DTO.OrderApiDto;
using Ecommerce.WebApi.DTO.UserApiDto;

namespace Ecommerce.WebApi.MapperProfiles
{
    public class UserApiProfile : Profile
    {
        public UserApiProfile()
        {
            #region AddUser
            CreateMap<AddUserApiRequestDto, AddUserDtoRequest>();

            CreateMap<AddUserApiResponseDto, AddUserDtoResponse>();
            #endregion

            #region GetUserById
            CreateMap<GetUserByIdApiRequestDto, GetUserByIdDtoRequest>();

            CreateMap<GetUserByIdDtoResponse, GetUserByIdApiResponseDto>();
            #endregion

            #region GetAllUsers

            CreateMap<UsersResponse, UsersApiResponse>();
            CreateMap<GetAllUsersDtoResponse, GetAllUsersApiResponseDto>();//ate qe kemi, ne ate qe do mapohet.
            #endregion

            #region UpdateUser
            CreateMap<UpdateUserApiRequestDto, UpdateUserDtoRequest>();

            CreateMap<UpdateUserDtoResponse, UpdateUserApiResponseDto>();
            #endregion

            #region DeleteOrder
            CreateMap<DeleteUserApiRequestDto, DeleteUserDtoRequest>();

            CreateMap<DeleteUserApiResponseDto, DeleteUserDtoResponse>();
            #endregion

            #region Login
            CreateMap<UserDtoApp, UserApiDto>();
            #endregion
        }
    }
}
