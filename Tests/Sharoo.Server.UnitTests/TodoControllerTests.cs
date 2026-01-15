using Microsoft.AspNetCore.Mvc;
using Moq;
using Sharoo.Server.API.Controllers;
using Sharoo.Server.Application.DTOs.Todos.Create.Request;
using Sharoo.Server.Application.Services.Todos;
using Sharoo.Server.Domain.Entities;
using Sharoo.Server.Domain.Exceptions;

namespace Sharoo.Server.UnitTests
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoService> _serviceMock;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _serviceMock = new Mock<ITodoService>();
            _controller = new TodoController(_serviceMock.Object);
        }

        #region GetAll Tests
        [Fact]
        public async Task GetAll_WhenTodosExist_ReturnsOkWithTodos()
        {
            var todos = new List<Todo>
            {
                new Todo { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Todo 1", IsDone = false, CreatedAt = DateTime.UtcNow },
                new Todo { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Todo 2", IsDone = true, CreatedAt = DateTime.UtcNow }
            };

            _serviceMock
                .Setup(s => s.ReadAsync())
                .ReturnsAsync(todos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _serviceMock.Verify(s => s.ReadAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenNoTodosExist_ReturnsEmptyList()
        {
            _serviceMock
                .Setup(s => s.ReadAsync())
                .ReturnsAsync(new List<Todo>());

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            _serviceMock.Verify(s => s.ReadAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsCorrectNumberOfTodos()
        {
            var todos = new List<Todo>
            {
                new Todo { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Todo 1", IsDone = false, CreatedAt = DateTime.UtcNow },
                new Todo { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Todo 2", IsDone = true, CreatedAt = DateTime.UtcNow },
                new Todo { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Todo 3", IsDone = false, CreatedAt = DateTime.UtcNow }
            };

            _serviceMock
                .Setup(s => s.ReadAsync())
                .ReturnsAsync(todos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var response = okResult.Value;
            Assert.NotNull(response);

            _serviceMock.Verify(s => s.ReadAsync(), Times.Once);
        }
        #endregion

        #region GetById Tests
        [Fact]
        public async Task GetById_WithValidId_ReturnsOkWithTodo()
        {
            var todoId = Guid.NewGuid();
            var todo = new Todo { Id = todoId, Name = "Test Todo", UserId = Guid.NewGuid(), IsDone = false, CreatedAt = DateTime.UtcNow };

            _serviceMock
                .Setup(s => s.ReadByIdAsync(todoId))
                .ReturnsAsync(todo);

            var result = await _controller.GetById(todoId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _serviceMock.Verify(s => s.ReadByIdAsync(todoId), Times.Once);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ThrowsTodoNotFoundException()
        {
            var invalidId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.ReadByIdAsync(invalidId))
                .ThrowsAsync(new TodoNotFoundException());

            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _controller.GetById(invalidId));
        }

        [Fact]
        public async Task GetById_WithNullResult_ReturnsNotFound()
        {
            var todoId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.ReadByIdAsync(todoId))
                .ReturnsAsync((Todo)null);

            var result = await _controller.GetById(todoId);

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Create Tests
        [Fact]
        public async Task Create_WithValidRequest_ReturnsCreated()
        {
            var request = new TodoCreateRequest { Title = "New Todo" };

            _serviceMock
                .Setup(s => s.CreateAsync(It.IsAny<Todo>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Create(request);

            Assert.IsType<CreatedResult>(result);
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<Todo>()), Times.Once);
        }

        [Fact]
        public async Task Create_WithInvalidRequest_ReturnsBadRequest()
        {
            var request = new TodoCreateRequest { Title = "" };
            _controller.ModelState.AddModelError("Name", "Name is required");

            var result = await _controller.Create(request);

            Assert.IsType<BadRequestObjectResult>(result);
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<Todo>()), Times.Never);
        }
        #endregion

        #region ChangeStatus Tests
        [Fact]
        public async Task ChangeStatus_WithValidId_ReturnsNoContent()
        {
            var todoId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.ChangeStatusAsync(todoId))
                .Returns(Task.CompletedTask);

            var result = await _controller.ChangeStatus(todoId);

            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.ChangeStatusAsync(todoId), Times.Once);
        }

        [Fact]
        public async Task ChangeStatus_WithInvalidId_ThrowsTodoNotFoundException()
        {
            var invalidId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.ChangeStatusAsync(invalidId))
                .ThrowsAsync(new TodoNotFoundException());

            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _controller.ChangeStatus(invalidId));
        }
        #endregion

        #region Delete Tests
        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            var todoId = Guid.NewGuid();
            var todo = new Todo { Id = todoId, Name = "Test Todo", UserId = Guid.NewGuid(), IsDone = false, CreatedAt = DateTime.UtcNow };

            _serviceMock
                .Setup(s => s.ReadByIdAsync(todoId))
                .ReturnsAsync(todo);
            _serviceMock
                .Setup(s => s.DeleteAsync(todoId))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(todoId);

            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(todoId), Times.Once);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            var invalidId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.ReadByIdAsync(invalidId))
                .ReturnsAsync((Todo)null);

            var result = await _controller.Delete(invalidId);

            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsTodoNotFoundException_WhenServiceThrows()
        {
            var todoId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.ReadByIdAsync(todoId))
                .ThrowsAsync(new TodoNotFoundException());

            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _controller.Delete(todoId));
        }
        #endregion
    }
}