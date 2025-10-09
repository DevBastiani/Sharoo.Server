﻿using Microsoft.EntityFrameworkCore;
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
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email && user.Password == password);
        }
    }
}
