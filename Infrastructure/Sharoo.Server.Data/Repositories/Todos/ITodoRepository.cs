using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Data.Repositories.Todos
{
    public interface ITodoRepository
    {
        Task CreateAsync(Todo todo);
        Task<Todo?> ReadByIdAsync(Guid todoId);
        Task<List<Todo>> ReadAsync(Guid userId);
        Task<List<Todo>> ReadByFilterAsync(
            Guid userId,
            DateTime? createdFrom,
            DateTime? createdTo,
            DateTime? completedFrom,
            DateTime? completedTo);
        Task DeleteAsync(Todo todo);
        Task ChangeStatusAsync(Todo todo);
    }
}
