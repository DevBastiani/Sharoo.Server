using Moq;
using Sharoo.Server.Domain.Entities;
using Sharoo.Server.Domain.Exceptions;

namespace Sharoo.Server.UnitTests
{
    public class TodoServiceTests : IClassFixture<TodoServiceFixture>
    {
        private readonly TodoServiceFixture _fixture;

        public TodoServiceTests(TodoServiceFixture fixture)
        {
            _fixture = fixture;
            _fixture.RepositoryMock.Reset();
            _fixture.NotificationServiceMock.Reset();
        }

        #region ReadAsync Tests
        [Fact]
        public async Task ReadAsync_WhenTodosExist_ReturnsAllTodos()
        {
            var todos = _fixture.CreateMultipleTodos(3);
            _fixture.RepositoryMock
                .Setup(r => r.ReadAsync())
                .ReturnsAsync(todos);

            var result = await _fixture.Service.ReadAsync();

            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);
            _fixture.RepositoryMock.Verify(r => r.ReadAsync(), Times.Once);
        }

        [Fact]
        public async Task ReadAsync_WhenNoTodosExist_ReturnsEmptyList()
        {
            _fixture.RepositoryMock
                .Setup(r => r.ReadAsync())
                .ReturnsAsync(new List<Todo>());

            var result = await _fixture.Service.ReadAsync();

            Assert.Empty(result);
            _fixture.RepositoryMock.Verify(r => r.ReadAsync(), Times.Once);
        }
        #endregion

        #region ReadByIdAsync Tests
        [Fact]
        public async Task ReadByIdAsync_WithValidId_ReturnsTodo()
        {
            var todo = _fixture.CreateTodoEntity();
            _fixture.RepositoryMock
                .Setup(r => r.ReadByIdAsync(todo.Id))
                .ReturnsAsync(todo);

            var result = await _fixture.Service.ReadByIdAsync(todo.Id);

            Assert.NotNull(result);
            Assert.Equal(todo.Id, result.Id);
            Assert.Equal(todo.Name, result.Name);
            _fixture.RepositoryMock.Verify(r => r.ReadByIdAsync(todo.Id), Times.Once);
        }

        [Fact]
        public async Task ReadByIdAsync_WithInvalidId_ThrowsTodoNotFoundException()
        {
            var invalidId = Guid.NewGuid();
            _fixture.RepositoryMock
                .Setup(r => r.ReadByIdAsync(invalidId))
                .ReturnsAsync((Todo)null);

            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _fixture.Service.ReadByIdAsync(invalidId));

            _fixture.RepositoryMock.Verify(r => r.ReadByIdAsync(invalidId), Times.Once);
        }
        #endregion

        #region CreateAsync Tests
        [Fact]
        public async Task CreateAsync_WithValidTodo_CreatesAndNotifies()
        {
            var todo = _fixture.CreateTodoEntity("New Todo");
            _fixture.RepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Todo>()))
                .Returns(Task.CompletedTask);
            _fixture.NotificationServiceMock
                .Setup(n => n.NotifyTodoCreatedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);

            await _fixture.Service.CreateAsync(todo);

            _fixture.RepositoryMock.Verify(
                r => r.CreateAsync(It.Is<Todo>(t => t.Name == "New Todo")),
                Times.Once);
            _fixture.NotificationServiceMock.Verify(
                n => n.NotifyTodoCreatedAsync(todo.Id, "New Todo", false, It.IsAny<DateTime>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithNullName_ThrowsArgumentException()
        {
            var todo = new Todo { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = null };

            await Assert.ThrowsAsync<ArgumentException>(
                () => _fixture.Service.CreateAsync(todo));

            _fixture.RepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Todo>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WithEmptyName_ThrowsArgumentException()
        {
            var todo = new Todo { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "   " };

            await Assert.ThrowsAsync<ArgumentException>(
                () => _fixture.Service.CreateAsync(todo));

            _fixture.RepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Todo>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_StartsNewTodo()
        {
            var todo = _fixture.CreateTodoEntity("New Todo");
            var initialCreatedAt = todo.CreatedAt;

            _fixture.RepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Todo>()))
                .Returns(Task.CompletedTask);
            _fixture.NotificationServiceMock
                .Setup(n => n.NotifyTodoCreatedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);

            await _fixture.Service.CreateAsync(todo);

            Assert.NotEqual(initialCreatedAt, todo.CreatedAt);
            _fixture.RepositoryMock.Verify(r => r.CreateAsync(todo), Times.Once);
        }
        #endregion

        #region DeleteAsync Tests
        [Fact]
        public async Task DeleteAsync_WithValidId_DeletesAndNotifies()
        {
            var todo = _fixture.CreateTodoEntity();
            _fixture.RepositoryMock
                .Setup(r => r.ReadByIdAsync(todo.Id))
                .ReturnsAsync(todo);
            _fixture.RepositoryMock
                .Setup(r => r.DeleteAsync(It.IsAny<Todo>()))
                .Returns(Task.CompletedTask);
            _fixture.NotificationServiceMock
                .Setup(n => n.NotifyTodoDeletedAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            await _fixture.Service.DeleteAsync(todo.Id);

            _fixture.RepositoryMock.Verify(r => r.DeleteAsync(todo), Times.Once);
            _fixture.NotificationServiceMock.Verify(
                n => n.NotifyTodoDeletedAsync(todo.Id),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ThrowsTodoNotFoundException()
        {
            var invalidId = Guid.NewGuid();
            _fixture.RepositoryMock
                .Setup(r => r.ReadByIdAsync(invalidId))
                .ReturnsAsync((Todo)null);

            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _fixture.Service.DeleteAsync(invalidId));

            _fixture.RepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Todo>()), Times.Never);
        }
        #endregion

        #region ChangeStatusAsync Tests
        [Fact]
        public async Task ChangeStatusAsync_WhenTodoIsNotDone_MarksAsDone()
        {
            var todo = _fixture.CreateTodoEntity("Test", isDone: false);
            _fixture.RepositoryMock
                .Setup(r => r.ReadByIdAsync(todo.Id))
                .ReturnsAsync(todo);
            _fixture.RepositoryMock
                .Setup(r => r.ChangeStatusAsync(It.IsAny<Todo>()))
                .Returns(Task.CompletedTask);
            _fixture.NotificationServiceMock
                .Setup(n => n.NotifyTodoStatusChangedAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            await _fixture.Service.ChangeStatusAsync(todo.Id);

            Assert.True(todo.IsDone);
            Assert.NotNull(todo.CompletedAt);
            _fixture.RepositoryMock.Verify(r => r.ChangeStatusAsync(todo), Times.Once);
            _fixture.NotificationServiceMock.Verify(
                n => n.NotifyTodoStatusChangedAsync(todo.Id, true),
                Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_WhenTodoIsDone_MarksAsNotDone()
        {
            var todo = _fixture.CreateTodoEntity("Test", isDone: true);
            todo.MarkAsDone();

            _fixture.RepositoryMock
                .Setup(r => r.ReadByIdAsync(todo.Id))
                .ReturnsAsync(todo);
            _fixture.RepositoryMock
                .Setup(r => r.ChangeStatusAsync(It.IsAny<Todo>()))
                .Returns(Task.CompletedTask);
            _fixture.NotificationServiceMock
                .Setup(n => n.NotifyTodoStatusChangedAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            await _fixture.Service.ChangeStatusAsync(todo.Id);

            Assert.False(todo.IsDone);
            Assert.Null(todo.CompletedAt);
            _fixture.RepositoryMock.Verify(r => r.ChangeStatusAsync(todo), Times.Once);
            _fixture.NotificationServiceMock.Verify(
                n => n.NotifyTodoStatusChangedAsync(todo.Id, false),
                Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_WithInvalidId_ThrowsTodoNotFoundException()
        {
            var invalidId = Guid.NewGuid();
            _fixture.RepositoryMock
                .Setup(r => r.ReadByIdAsync(invalidId))
                .ReturnsAsync((Todo)null);

            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _fixture.Service.ChangeStatusAsync(invalidId));

            _fixture.RepositoryMock.Verify(r => r.ChangeStatusAsync(It.IsAny<Todo>()), Times.Never);
            _fixture.NotificationServiceMock.Verify(
                n => n.NotifyTodoStatusChangedAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                Times.Never);
        }
        #endregion
    }
}