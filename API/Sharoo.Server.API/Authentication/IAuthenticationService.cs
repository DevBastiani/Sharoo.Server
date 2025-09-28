using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.API.Authentication
{
    public interface IAuthenticationService
    {
        string GenerateToken(User user);
    }
}
