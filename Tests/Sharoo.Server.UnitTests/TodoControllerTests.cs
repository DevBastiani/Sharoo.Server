using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sharoo.Server.API.Controllers;
using Sharoo.Server.Application.DTOs.Todos.Create.Request;
using Sharoo.Server.Application.Services.Todos;
using Sharoo.Server.Domain.Entities;
using Sharoo.Server.Domain.Exceptions;
using System.Security.Claims;

namespace Sharoo.Server.UnitTests
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoService> _serviceMock;
        private readonly TodoController _controller;
        private readonly Guid _userId;

        public TodoControllerTests()
        {
            _serviceMock = new Mock<ITodoService>();
            _controller = new TodoController(_serviceMock.Object);
            _userId = Guid.NewGuid();

            var claims = new List<Claim>
            {
                new Claim("Id", _userId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "Bearer");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        #region GetAll Tests
        [Fact]
        public async Task GetAll_WhenTodosExist_ReturnsOkWithTodos()
        {
            var todos = new List<Todo>
            {
                new Todo { Id = Guid.NewGuid(), UserId = _userId, Name = "Todo 1", IsDone = false, CreatedAt = DateTime.UtcNow },
                new Todo { Id = Guid.NewGuid(), UserId = _userId, Name = "Todo 2", IsDone = true, CreatedAt = DateTime.UtcNow }
            };

            _serviceMock
                .Setup(s => s.ReadAsync(_userId))
                .ReturnsAsync(todos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _serviceMock.Verify(s => s.ReadAsync(_userId), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenNoTodosExist_ReturnsEmptyList()
        {
            _serviceMock
                .Setup(s => s.ReadAsync(_userId))
                .ReturnsAsync(new List<Todo>());

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            _serviceMock.Verify(s => s.ReadAsync(_userId), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsCorrectNumberOfTodos()
        {
            var todos = new List<Todo>
            {
                new Todo { Id = Guid.NewGuid(), UserId = _userId, Name = "Todo 1", IsDone = false, CreatedAt = DateTime.UtcNow },
                new Todo { Id = Guid.NewGuid(), UserId = _userId, Name = "Todo 2", IsDone = true, CreatedAt = DateTime.UtcNow },
                new Todo { Id = Guid.NewGuid(), UserId = _userId, Name = "Todo 3", IsDone = false, CreatedAt = DateTime.UtcNow }
            };

            _serviceMock
                .Setup(s => s.ReadAsync(_userId))
                .ReturnsAsync(todos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            _serviceMock.Verify(s => s.ReadAsync(_userId), Times.Once);
        }
        #endregion

        #region GetById Tests
        [Fact]
        public async Task GetById_WithValidId_ReturnsOkWithTodo()
        {
            var todoId = Guid.NewGuid();
            var todo = new Todo { Id = todoId, Name = "Test Todo", UserId = _userId, IsDone = false, CreatedAt = DateTime.UtcNow };

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

            _serviceMock
                .Setup(s => s.DeleteAsync(todoId))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(todoId);

            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(todoId), Times.Once);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ThrowsTodoNotFoundException()
        {
            var invalidId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.DeleteAsync(invalidId))
                .ThrowsAsync(new TodoNotFoundException());

            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _controller.Delete(invalidId));
        }
        #endregion
    }
}
