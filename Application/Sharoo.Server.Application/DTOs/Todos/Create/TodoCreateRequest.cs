using Sharoo.Server.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sharoo.Server.Application.DTOs.Todos.Create.Request
{
    public class TodoCreateRequest
    {
        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [MaxLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres.")]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres.")]
        public string? Description { get; set; }

        public static Todo FromRequestToEntity(TodoCreateRequest request)
        {
            return new Todo
            {
                Name = request.Title,
                Description = request.Description
            };
        }
    }
}