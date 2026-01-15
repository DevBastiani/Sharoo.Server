using Microsoft.AspNetCore.SignalR;
using Moq;
using Sharoo.Server.API.Hubs;
using Sharoo.Server.API.Services;

namespace Sharoo.Server.UnitTests
{
    public class TodoNotificationServiceTests
    {
        private readonly Mock<IHubContext<TodoHub>> _hubContextMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly TodoNotificationService _service;

        public TodoNotificationServiceTests()
        {
            _hubContextMock = new Mock<IHubContext<TodoHub>>();
            _clientProxyMock = new Mock<IClientProxy>();

            _hubContextMock
                .Setup(h => h.Clients.All)
                .Returns(_clientProxyMock.Object);

            _service = new TodoNotificationService(_hubContextMock.Object);
        }

        #region NotifyTodoCreatedAsync Tests
        [Fact]
        public async Task NotifyTodoCreatedAsync_SendsCreatedNotificationToAllClients()
        {
            var todoId = Guid.NewGuid();
            var name = "New Todo";
            var isDone = false;
            var createdAt = DateTime.UtcNow;

            _clientProxyMock
                .Setup(c => c.SendCoreAsync("TodoCreated", It.IsAny<object[]>(), default))
                .Returns(Task.CompletedTask);

            await _service.NotifyTodoCreatedAsync(todoId, name, isDone, createdAt);

            _clientProxyMock.Verify(
                c => c.SendCoreAsync("TodoCreated", It.Is<object[]>(o => o.Length == 1), default),
                Times.Once);
        }

        [Fact]
        public async Task NotifyTodoCreatedAsync_IncludesAllRequiredFields()
        {
            var todoId = Guid.NewGuid();
            var name = "Test Todo";
            var isDone = true;
            var createdAt = DateTime.UtcNow;

            object[] capturedArgs = null;

            _clientProxyMock
                .Setup(c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default))
                .Callback<string, object[], CancellationToken>((method, args, ct) => capturedArgs = args)
                .Returns(Task.CompletedTask);

            await _service.NotifyTodoCreatedAsync(todoId, name, isDone, createdAt);

            Assert.NotNull(capturedArgs);
            Assert.Single(capturedArgs);
            var capturedObject = capturedArgs[0];
            var properties = capturedObject.GetType().GetProperties();
            Assert.Equal(4, properties.Length);
        }

        [Fact]
        public async Task NotifyTodoCreatedAsync_SendsCorrectData()
        {
            var todoId = Guid.NewGuid();
            var name = "Test Todo";
            var isDone = true;
            var createdAt = new DateTime(2026, 1, 15, 10, 30, 0);

            object[] capturedArgs = null;
            string capturedMethod = null;

            _clientProxyMock
                .Setup(c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default))
                .Callback<string, object[], CancellationToken>((method, args, ct) =>
                {
                    capturedMethod = method;
                    capturedArgs = args;
                })
                .Returns(Task.CompletedTask);

            await _service.NotifyTodoCreatedAsync(todoId, name, isDone, createdAt);

            Assert.Equal("TodoCreated", capturedMethod);
            Assert.NotNull(capturedArgs);
            Assert.Single(capturedArgs);

            var capturedData = capturedArgs[0];
            var type = capturedData.GetType();
            var idProp = type.GetProperty("id")?.GetValue(capturedData);
            var nameProp = type.GetProperty("name")?.GetValue(capturedData);
            var isdoneProp = type.GetProperty("isDone")?.GetValue(capturedData);
            var createdAtProp = type.GetProperty("createdAt")?.GetValue(capturedData);

            Assert.Equal(todoId, idProp);
            Assert.Equal(name, nameProp);
            Assert.Equal(isDone, isdoneProp);
            Assert.Equal(createdAt, createdAtProp);
        }
        #endregion

        #region NotifyTodoStatusChangedAsync Tests
        [Fact]
        public async Task NotifyTodoStatusChangedAsync_SendsStatusChangeNotification()
        {
            var todoId = Guid.NewGuid();
            var isDone = true;

            _clientProxyMock
                .Setup(c => c.SendCoreAsync("TodoStatusChanged", It.IsAny<object[]>(), default))
                .Returns(Task.CompletedTask);

            await _service.NotifyTodoStatusChangedAsync(todoId, isDone);

            _clientProxyMock.Verify(
                c => c.SendCoreAsync("TodoStatusChanged", It.IsAny<object[]>(), default),
                Times.Once);
        }

        [Fact]
        public async Task NotifyTodoStatusChangedAsync_IncludesIdAndStatus()
        {
            var todoId = Guid.NewGuid();
            var isDone = false;

            object[] capturedArgs = null;

            _clientProxyMock
                .Setup(c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default))
                .Callback<string, object[], CancellationToken>((method, args, ct) => capturedArgs = args)
                .Returns(Task.CompletedTask);

            await _service.NotifyTodoStatusChangedAsync(todoId, isDone);

            Assert.NotNull(capturedArgs);
            Assert.Single(capturedArgs);
            var capturedObject = capturedArgs[0];
            var properties = capturedObject.GetType().GetProperties();
            Assert.Equal(2, properties.Length);
        }
        #endregion

        #region NotifyTodoDeletedAsync Tests
        [Fact]
        public async Task NotifyTodoDeletedAsync_SendsDeleteNotification()
        {
            var todoId = Guid.NewGuid();

            _clientProxyMock
                .Setup(c => c.SendCoreAsync("TodoDeleted", It.IsAny<object[]>(), default))
                .Returns(Task.CompletedTask);

            await _service.NotifyTodoDeletedAsync(todoId);

            _clientProxyMock.Verify(
                c => c.SendCoreAsync("TodoDeleted", It.Is<object[]>(args => args.Length == 1 && args[0].Equals(todoId)), default),
                Times.Once);
        }

        [Fact]
        public async Task NotifyTodoDeletedAsync_SendsCorrectTodoId()
        {
            var todoId = Guid.NewGuid();

            object[] capturedArgs = null;

            _clientProxyMock
                .Setup(c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default))
                .Callback<string, object[], CancellationToken>((method, args, ct) => capturedArgs = args)
                .Returns(Task.CompletedTask);

            await _service.NotifyTodoDeletedAsync(todoId);

            Assert.NotNull(capturedArgs);
            Assert.Single(capturedArgs);
            Assert.Equal(todoId, capturedArgs[0]);
        }
        #endregion
    }
}