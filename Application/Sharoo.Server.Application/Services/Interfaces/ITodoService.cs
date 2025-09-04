using Sharoo.Server.Application.DTOs.Todos.Request;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Application.Services.Interfaces
{
    public interface ITodoService
    {
        public Task CreateAsync(Todo todo);
        public Task<Todo> ReadByIdAsync(Guid todoId);
        public Task<List<Todo>> ReadAsync();
        public Task DeleteAsync(Guid todoId);
        public Task ChangeStatusAsync(TodoChangeStatusRequest todoId);
    }
}
