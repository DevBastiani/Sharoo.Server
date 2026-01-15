using Moq;
using Sharoo.Server.Application.Services.Notifications;
using Sharoo.Server.Application.Services.Todos;
using Sharoo.Server.Data.Repositories.Todos;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Tests.Fixtures
{
    public class TodoServiceFixture : IDisposable
    {
        public Mock<ITodoRepository> RepositoryMock { get; private set; }
        public Mock<ITodoNotificationService> NotificationServiceMock { get; private set; }
        public TodoService Service { get; private set; }

        public TodoServiceFixture()
        {
            RepositoryMock = new Mock<ITodoRepository>();
            NotificationServiceMock = new Mock<ITodoNotificationService>();
            Service = new TodoService(RepositoryMock.Object, NotificationServiceMock.Object);
        }

        public Todo CreateTodoEntity(string name = "Test Todo", bool isDone = false)
        {
            return new Todo 
            { 
                Id = Guid.NewGuid(), 
                Name = name,
                IsDone = isDone,
                CreatedAt = DateTime.UtcNow
            };
        }

        public List<Todo> CreateMultipleTodos(int count = 3)
        {
            return Enumerable.Range(1, count)
                .Select(i => CreateTodoEntity($"Todo {i}"))
                .ToList();
        }

        public void Dispose()
        {
            RepositoryMock?.Reset();
            NotificationServiceMock?.Reset();
        }
    }
}