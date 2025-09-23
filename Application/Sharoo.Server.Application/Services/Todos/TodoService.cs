using Sharoo.Server.Application.DTOs.Todos.Request;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Application.Services.Todos
{
    public class TodoService : ITodoService
    {
        public Task ChangeStatusAsync(TodoChangeStatusRequest todoId)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Todo todo)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid todoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Todo>> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Todo> ReadByIdAsync(Guid todoId)
        {
            throw new NotImplementedException();
        }
    }
}
