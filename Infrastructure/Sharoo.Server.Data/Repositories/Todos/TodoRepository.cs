using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Data.Repositories.Todos
{
    public class TodoRepository : ITodoRepository
    {
        public Task ChangeStatusAsync(Guid todoId, bool isDone)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Task todo)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid todoId)
        {
            throw new NotImplementedException();
        }

        public Task<Todo?> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public Task ReadByIdAsync(Guid todoId)
        {
            throw new NotImplementedException();
        }
    }
}
