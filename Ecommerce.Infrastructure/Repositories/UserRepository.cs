using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Ecommerce.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users
                                 .Include(u => u.Role)
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users
                                 .Include(u => u.Role)
                                 .FirstOrDefaultAsync(u => u.Username == username);

        }
        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }
        public async Task AddAUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
