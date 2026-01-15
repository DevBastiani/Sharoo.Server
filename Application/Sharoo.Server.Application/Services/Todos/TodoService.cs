using Sharoo.Server.Application.Services.Notifications;
using Sharoo.Server.Data.Repositories.Todos;
using Sharoo.Server.Domain.Entities;
using Sharoo.Server.Domain.Exceptions;

namespace Sharoo.Server.Application.Services.Todos
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly ITodoNotificationService _notificationService;

        public TodoService(ITodoRepository repository, ITodoNotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task ChangeStatusAsync(Guid todoId)
        {
            var todo = await _repository.ReadByIdAsync(todoId);
            if (todo is null) throw new TodoNotFoundException();

            if (todo.IsDone)
                todo.MarkAsNotDone();
            else
                todo.MarkAsDone();

            await _repository.ChangeStatusAsync(todo);

            await _notificationService.NotifyTodoStatusChangedAsync(todo.Id, todo.IsDone);
        }

        public async Task CreateAsync(Todo todo)
        {
            if (string.IsNullOrWhiteSpace(todo.Name))
                throw new ArgumentException("Nome do TODO é obrigatório.");

            todo.Start();
            await _repository.CreateAsync(todo);

            await _notificationService.NotifyTodoCreatedAsync(todo.Id, todo.Name, todo.IsDone, todo.CreatedAt);
        }

        public async Task DeleteAsync(Guid todoId)
        {
            var todo = await _repository.ReadByIdAsync(todoId);
            if (todo is null) throw new TodoNotFoundException();

            await _repository.DeleteAsync(todo);

            await _notificationService.NotifyTodoDeletedAsync(todoId);
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
