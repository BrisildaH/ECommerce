using AutoMapper;
using Ecommerce.Application.DTO.OrderDto;
using Ecommerce.Application.DTO.UserDto;
using Ecommerce.Domain;
using Ecommerce.WebApi.DTO.UserApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            #region AddUser
            CreateMap<AddUserDtoRequest, User>();

            CreateMap<User, AddUserDtoResponse>();
            #endregion

            #region GetUserById
            CreateMap<GetUserByIdDtoRequest, User>();

            CreateMap<User, GetUserByIdDtoResponse>();
            CreateMap<User, GetUserByUsernameDtoResponse>();
            #endregion

            #region GetAllUsers

            CreateMap<User, UsersResponse>();

            CreateMap<List<User>, GetAllUsersDtoResponse>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src));
            #endregion

            #region UpdateUser
            CreateMap<UpdateUserDtoRequest, User>();

            CreateMap<User, UpdateUserDtoResponse>();
            #endregion

            #region DeleteUser
            CreateMap<DeleteUserDtoRequest, User>();

            CreateMap<User, DeleteUserDtoResponse>();
            #endregion


            #region Log In
            CreateMap<User, UserDtoApp>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            #endregion

        }
    }
}
