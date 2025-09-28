using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Application.Services.Users
{
    public interface IUserService
    {
        Task CreateUserAsync(User user);
        Task<User> ReadUserByEmailAndPasswordAsync(string email, string password);
    }
}
