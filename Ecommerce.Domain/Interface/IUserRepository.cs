using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interface
{
    public interface IUserRepository
    {
      Task<User> GetUserById(int id);
      Task<User> GetUserByUsername(string username);
      IQueryable<User> GetAll();
      Task AddAUser(User user);
      Task UpdateUser(User user);
      Task DeleteUser(int id);

    }
}
