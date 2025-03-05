using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.DTO.UserApiDto
{
    public class GetAllUsersApiResponseDto
    {
        public List<UsersApiResponse> Users { get; set; }  
    }
}
