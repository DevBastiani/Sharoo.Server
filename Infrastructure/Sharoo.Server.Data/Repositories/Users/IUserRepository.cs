using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Data.Repositories.Users
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task<User?> GetByEmailAndPasswordAsync(string email, string password);
    }
}
