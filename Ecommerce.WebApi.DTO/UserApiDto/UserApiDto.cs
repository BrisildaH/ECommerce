using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.UserApiDto
{
    public class UserApiDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        //public string Password { get; set; }  
        public string RoleName { get; set; }
    }
}
