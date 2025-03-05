namespace Ecommerce.Application.DTO.UserDto
{
    public class AddUserDtoRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } 
    }
}

