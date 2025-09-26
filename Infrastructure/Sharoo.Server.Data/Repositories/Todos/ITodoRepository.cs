using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Data.Repositories.Todos
{
    public interface ITodoRepository
    {
        Task CreateAsync(Todo todo);
        Task<Todo?> ReadByIdAsync(Guid todoId);
        Task<List<Todo>> ReadAsync();
        Task DeleteAsync(Todo todo);
        Task ChangeStatusAsync(Todo todo);
    }
}
