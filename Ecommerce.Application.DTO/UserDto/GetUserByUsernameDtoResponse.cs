using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO.UserDto
{
    public class GetUserByUsernameDtoResponse
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
