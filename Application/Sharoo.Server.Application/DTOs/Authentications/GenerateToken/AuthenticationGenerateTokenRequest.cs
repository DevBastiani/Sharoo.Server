using System.ComponentModel.DataAnnotations;

namespace Sharoo.Server.Application.DTOs.Authentications.GenerateToken
{
    public class AuthenticationGenerateTokenRequest
    {
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public required string Password { get; set; }
    }
}
