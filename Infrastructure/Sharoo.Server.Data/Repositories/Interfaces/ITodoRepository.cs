using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Data.Repositories.Interfaces
{
    public interface ITodoRepository
    {
        public Task CreateAsync(Task todo);
        public Task ReadByIdAsync(Guid todoId);
        public Task<Todo?> ReadAsync();
        public Task DeleteAsync(Guid todoId);
        public Task ChangeStatusAsync(Guid todoId, bool isDone);
    }
}
