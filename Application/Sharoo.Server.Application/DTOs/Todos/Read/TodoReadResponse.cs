namespace Sharoo.Server.Application.DTOs.Todos.Read
{
    public class TodoReadResponse
    {
        public List<TodoReadItemResponse> Todos { get; set; } = new();

        public static TodoReadResponse FromEntitiesToResponse(List<Domain.Entities.Todo> entities)
        {
            var response = new TodoReadResponse();
            response.Todos = entities.Select(entity => new TodoReadItemResponse
            {
                Id = entity.Id,
                Title = entity.Name,
                Description = entity.Description,
                IsDone = entity.IsDone,
                CreatedAt = entity.CreatedAt,
                CompletedAt = entity.CompletedAt
            }).ToList();

            return response;
        }
    }
}
