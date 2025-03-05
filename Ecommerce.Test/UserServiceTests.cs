using AutoMapper;
using Ecommerce.Application.DTO.UserDto;
using Ecommerce.Application.Interface;
using Ecommerce.Application.Resources;
using Ecommerce.Application.Services;
using Ecommerce.Common.Exceptions;
using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Ecommerce.Test
{
    [TestClass]
    public class UserServiceTests
    {
        public IUserRepository _userRepository;
        private IMapper _mapper;
        private IPasswordHasher _passwordHasher;
        private UserService _userService;
        private IConfiguration _configuration;

        [TestInitialize]
        public void Setup()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
             _configuration = Substitute.For<IConfiguration>();
            _passwordHasher = Substitute.For<IPasswordHasher>();

            _userService = new UserService(_userRepository, _mapper, _passwordHasher, _configuration);
        }

        [TestMethod]
        public async Task GetUserById_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            int id = 1;
            _userRepository.GetUserById(id).Returns(Task.FromResult<User>(null));
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _userService.GetUserById(id));
        }

        [TestMethod]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            int userId = 1;
            var user = new User { Id = userId };
            var userDto = new GetUserByIdDtoResponse { };

            _userRepository.GetUserById(userId).Returns(Task.FromResult(user));
            _mapper.Map<GetUserByIdDtoResponse>(user).Returns(userDto);

            var result = await _userService.GetUserById(userId);
            Assert.AreEqual(userDto, result);
        }

        [TestMethod]
        public async Task GetUserByUsername_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            string username = "User";
            _userRepository.GetUserByUsername(username).Returns(Task.FromResult<User>(null));
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _userService.GetUserByUsername(username));
        }

        [TestMethod]
        public async Task GetUserByUsername_ShouldReturnUser_WhenUserExists()
        {
            var username = "user";
            var user = new User { Id = 1, Username = username };
            var userDto = new GetUserByUsernameDtoResponse { Username = username };

            _userRepository.GetUserByUsername(username).Returns(Task.FromResult(user));
            _mapper.Map<GetUserByUsernameDtoResponse>(user).Returns(userDto);

            var result = await _userService.GetUserByUsername(username);
            Assert.AreEqual(username, result.Username);
        }

        [TestMethod]
        public async Task AddUser_ShouldThrowForbiddenException_WhenUserRoleIsNotAdmin()
        {
            var adduserDto = new AddUserDtoRequest { Username = "testuser" };
            var userRole = "User";

            var exception = await Assert.ThrowsExceptionAsync<ForbiddenException>(() =>
                _userService.AddUser(adduserDto, userRole));

            Assert.AreEqual(StringResourceMessage.ForbiddenAddUserError, exception.Message); //ndoshta i panevojshem?
        }

        [TestMethod]
        public async Task AddUser_ShouldThrowBadRequestException_WhenUserAlreadyExists()
        {
            var adduserDto = new AddUserDtoRequest { Username = "testuser", RoleId = 1 };
            var userRole = "Admin";

            var existingUser = new User { Username = "testuser", RoleId = 1 };

            _userRepository.GetUserByUsername(adduserDto.Username).Returns(Task.FromResult(existingUser));

            var exception = await Assert.ThrowsExceptionAsync<BadRequestException>(() =>
                _userService.AddUser(adduserDto, userRole));

        }

        [TestMethod]
        public async Task AddUser_ShouldAddUser_WhenValidRequest()
        {
            var adduserDto = new AddUserDtoRequest { Username = "testuser", Password = "password123" };
            var userRole = "Admin";

            var newUser = new User { Id = 1, Username = "testuser" };
            var responseDto = new AddUserDtoResponse { Id = 1, Username = "testuser" };

            _userRepository.GetUserByUsername(adduserDto.Username).Returns(Task.FromResult<User>(null));
            _mapper.Map<User>(adduserDto).Returns(newUser);
            _passwordHasher.HashPassword(adduserDto.Password).Returns("hashedPassword");
            _mapper.Map<AddUserDtoResponse>(newUser).Returns(responseDto);

            var result = await _userService.AddUser(adduserDto, userRole);

            Assert.AreEqual(adduserDto.Username, result.Username);
        }

        [TestMethod]
        public async Task UpdateUser_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            var userId = 1;
            var updateUserDto = new UpdateUserDtoRequest { Username = "newusername" };

            _userRepository.GetUserById(userId).Returns(Task.FromResult<User>(null));

            var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _userService.UpdateUser(userId, updateUserDto));

            Assert.AreEqual(StringResourceMessage.NotFoundValueUser, exception.Message);
        }

        [TestMethod]
        public async Task UpdateUser_ShouldThrowBadRequestException_WhenUsernameExistsForAnotherUser()
        {
            var userId = 1;
            var updateUserDto = new UpdateUserDtoRequest { Username = "existingusername", RoleId = 2 };

            var existingUser = new User { Id = userId };
            var anotherUserWithSameUsername = new User { Id = 2, Username = "existingusername", RoleId = 2 };

            _userRepository.GetUserById(userId).Returns(Task.FromResult(existingUser));
            _userRepository.GetUserByUsername(updateUserDto.Username).Returns(Task.FromResult(anotherUserWithSameUsername));

            var exception = await Assert.ThrowsExceptionAsync<BadRequestException>(() =>
                _userService.UpdateUser(userId, updateUserDto));

        }

        [TestMethod]
        public async Task UpdateUser_ShouldUpdateUser_WhenValidRequest()
        {
            var userId = 1;
            var updateUserDto = new UpdateUserDtoRequest { Username = "newusername", Password = "newpassword", RoleId = 2 };

            var existingUser = new User { Id = userId, Username = "oldusername", Password = "oldPassword", RoleId = 1 };
            var responseDto = new UpdateUserDtoResponse { Username = "newusername" };


            _userRepository.GetUserById(userId).Returns(Task.FromResult(existingUser));
            _userRepository.GetUserByUsername(updateUserDto.Username).Returns(Task.FromResult<User>(null));
            _passwordHasher.HashPassword(updateUserDto.Password).Returns("hashedPassword");

            _mapper.Map<UpdateUserDtoResponse>(existingUser).Returns(responseDto);

            var result = await _userService.UpdateUser(userId, updateUserDto);

            Assert.AreEqual(updateUserDto.Username, result.Username);
        }

        [TestMethod]

        public async Task DeleteUser_ShouldDeleteUser_WhenValidRequest()
        {
            int userId = 1;
            int loggedInUserId = 2;

            var existingUser = new User
            {
                Id = userId,
                Username = "testUser"
            };

            var deletedUserDto = new DeleteUserDtoResponse
            {

            };

            _userRepository.GetUserById(userId).Returns(existingUser);
            _mapper.Map<DeleteUserDtoResponse>(existingUser).Returns(deletedUserDto);

            var result = await _userService.DeleteUser(userId, loggedInUserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(deletedUserDto, result);
            await _userRepository.Received(1).DeleteUser(userId);
        }

        [TestMethod]

        public async Task DeleteUser_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            int userId = 1;
            int loggedInUserId = 2;
            _userRepository.GetUserById(userId).Returns((User)null);
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _userService.DeleteUser(userId, loggedInUserId));
        }

        [TestMethod]
        public async Task DeleteUser_ShouldThrowBadRequestException_WhenDeletingOwnAccount()
        {
            int userId = 1;
            int loggedInUserId = 1;

            var existingUser = new User
            {
                Id = userId,
                Username = "testUser"
            };
            _userRepository.GetUserById(userId).Returns(existingUser);
            await Assert.ThrowsExceptionAsync<BadRequestException>(() =>
                                         _userService.DeleteUser(userId, loggedInUserId));
        }

        [TestMethod]
        public async Task AuthenticateAsync_ShouldReturnUserDtoApp_WhenValidCredentials() //nuk funksionon
        {
            var username = "username";
            var password = "password";
            var hashedPassword = "hashedPassword";

            var user = new User
            {
                Id = 1,
                Username = username,
                Password = hashedPassword,
            };

            var userDtoApp = new UserDtoApp
            {
                Id = user.Id,
                Username = user.Username,
                JwtToken = "fakeJwtToken"
            };
            _userRepository.GetUserByUsername(username).Returns(user);
            _passwordHasher.VerifyPassword(password, hashedPassword).Returns(true);

            var secretKeySection = Substitute.For<IConfigurationSection>(); //duke qene se perdoret vetem ne kete metode
            secretKeySection.Value.Returns("fakeSecretKey");
            _configuration.GetSection("Authentication:Jwt:SecretKey").Returns(secretKeySection);

            var issuerSection = Substitute.For<IConfigurationSection>();
            issuerSection.Value.Returns("fakeIssuer");
            _configuration.GetSection("Authentication:Jwt:Issuer").Returns(issuerSection);

            var audienceSection = Substitute.For<IConfigurationSection>();
            audienceSection.Value.Returns("fakeAudience");
            _configuration.GetSection("Authentication:Jwt:Audience").Returns(audienceSection);

            _mapper.Map<UserDtoApp>(user).Returns(userDtoApp);

            var result = await _userService.AuthenticateAsync(username, password);

            Assert.IsNotNull(result);
            Assert.AreEqual(userDtoApp.Username, result.Username);
            Assert.IsFalse(string.IsNullOrEmpty(result.JwtToken));
        }

        [TestMethod]
        public async Task AuthenticateAsync_ShouldThrowUnauthorizedAccessException_WhenPasswordIsIncorrect()
        {
            var username = "testUser";
            var password = "wrongPassword";
            var hashedPassword = "hashedPassword";

            var user = new User
            {
                Id = 1,
                Username = username,
                Password = hashedPassword
            };

            _userRepository.GetUserByUsername(username).Returns(user);
            _passwordHasher.VerifyPassword(password, hashedPassword).Returns(false);

             await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
                _userService.AuthenticateAsync(username, password)
            );
        }

        [TestMethod]
        public async Task AuthenticateAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            var username = "nonExistentUser";
            var password = "anyPassword";

            _userRepository.GetUserByUsername(username).Returns((User)null);

            await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
                _userService.AuthenticateAsync(username, password)
            );
        }
    }
}