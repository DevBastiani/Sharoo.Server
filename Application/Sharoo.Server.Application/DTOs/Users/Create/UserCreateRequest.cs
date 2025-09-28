using Sharoo.Server.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sharoo.Server.Application.DTOs.Users.Create
{
    public class UserCreateRequest
    {
        [Required(ErrorMessage = "O campo Name é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        public string Password { get; set; } = string.Empty;

        public static User FromRequestToEntity(UserCreateRequest request)
        {
            return new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };
        }
    }
}
