using Sharoo.Server.Data.Repositories.Users;
using Sharoo.Server.Domain.Entities;
using Sharoo.Server.Domain.Exceptions;
using Sharoo.Server.Domain.Utilities;

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
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Senha não pode ser vazia.");

            user.Password = PasswordHasher.HashPassword(user.Password);
            user.Start();

            await _repository.CreateAsync(user);
        }

        public async Task<User> ReadUserByEmailAndPasswordAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email e senha são obrigatórios.");

            var user = await _repository.GetByEmailAsync(email);

            if (user is null || !PasswordHasher.VerifyPassword(password, user.Password))
                throw new UserNotFoundException();

            return user;
        }
    }
}
