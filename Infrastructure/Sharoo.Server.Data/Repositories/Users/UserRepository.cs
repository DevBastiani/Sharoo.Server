using Microsoft.EntityFrameworkCore;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Data.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly SharooDbContext _context;

        public UserRepository(SharooDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
