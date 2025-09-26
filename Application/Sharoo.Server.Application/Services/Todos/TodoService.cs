using Sharoo.Server.Domain.Entities;
using Sharoo.Server.Data.Repositories.Todos;

namespace Sharoo.Server.Application.Services.Todos
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task ChangeStatusAsync(Guid todoId)
        {
            var todo = await _repository.ReadByIdAsync(todoId);
            if (todo is null) throw new Exception();

            await _repository.ChangeStatusAsync(todo);
        }

        public async Task CreateAsync(Todo todo)
        {
            await _repository.CreateAsync(todo);
        }

        public async Task DeleteAsync(Guid todoId)
        {
            var todo = await _repository.ReadByIdAsync(todoId);
            if (todo is null) throw new Exception();

            await _repository.DeleteAsync(todo);
        }

        public async Task<List<Todo>> ReadAsync()
        {
            return await _repository.ReadAsync();
        }

        public async Task<Todo?> ReadByIdAsync(Guid todoId)
        {
            Todo? todo = await _repository.ReadByIdAsync(todoId);
            if (todo is null) throw new Exception();

            return todo;
        }
    }
}
