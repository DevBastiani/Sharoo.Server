using Sharoo.Server.Data.Repositories.Todos;
using Sharoo.Server.Domain.Entities;
using Sharoo.Server.Domain.Exceptions;

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
            if (todo is null) throw new TodoNotFoundException();

            if (todo.IsDone) todo.MarkAsDone();
            else todo.MarkAsNotDone();

            await _repository.ChangeStatusAsync(todo);
        }

        public async Task CreateAsync(Todo todo)
        {
            if (string.IsNullOrWhiteSpace(todo.Name))
                throw new ArgumentException("Nome do TODO é obrigatório.");

            todo.Start();
            await _repository.CreateAsync(todo);
        }

        public async Task DeleteAsync(Guid todoId)
        {
            var todo = await _repository.ReadByIdAsync(todoId);
            if (todo is null) throw new TodoNotFoundException();

            await _repository.DeleteAsync(todo);
        }

        public async Task<List<Todo>> ReadAsync()
        {
            return await _repository.ReadAsync();
        }

        public async Task<Todo> ReadByIdAsync(Guid todoId)
        {
            var todo = await _repository.ReadByIdAsync(todoId);
            if (todo is null) throw new TodoNotFoundException();

            return todo;
        }
    }
}
