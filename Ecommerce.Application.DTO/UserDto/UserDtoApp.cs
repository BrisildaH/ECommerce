using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.UserDto
{
    public class UserDtoApp
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string JwtToken { get; set; }
    }
}
