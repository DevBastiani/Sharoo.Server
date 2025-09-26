using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Application.DTOs.Todos.ReadById
{
    public class TodoReadByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public static TodoReadByIdResponse FromEntityToResponse(Todo entity)
        {
            return new TodoReadByIdResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsDone = entity.IsDone,
                CreatedAt = entity.CreatedAt,
                CompletedAt = entity.CompletedAt
            };
        }
    }
}