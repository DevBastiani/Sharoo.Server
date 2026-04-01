using Sharoo.Server.Application.DTOs.Todos.Filter;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Application.Services.Todos
{
    public interface ITodoService
    {
        public Task CreateAsync(Todo todo);
        public Task<Todo> ReadByIdAsync(Guid todoId);
        public Task<List<Todo>> ReadAsync(Guid userId);
        public Task<List<Todo>> ReadByFilterAsync(Guid userId, TodoFilterRequest filter);
        public Task DeleteAsync(Guid todoId);
        public Task ChangeStatusAsync(Guid todoId);
    }
}
