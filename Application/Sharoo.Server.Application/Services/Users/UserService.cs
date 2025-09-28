using Sharoo.Server.Data.Repositories.Users;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateUserAsync(User user)
        {
            await _repository.CreateAsync(user);
        }

        public Task<User> ReadUserByEmailAndPasswordAsync(string email, string password)
        {
            var user = _repository.GetByEmailAndPasswordAsync(email, password);
            if (user is null) throw new Exception("Usuário não foi encontrado.");

            return user;
        }
    }
}
