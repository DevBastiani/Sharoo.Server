using Microsoft.AspNetCore.SignalR;
using Moq;
using Sharoo.Server.API.Hubs;

namespace Sharoo.Server.UnitTests
{
    public class TodoHubTests
    {
        private readonly Mock<HubCallerContext> _contextMock;
        private readonly Mock<IHubCallerClients> _clientsMock;
        private readonly Mock<IGroupManager> _groupsMock;
        private readonly TodoHub _hub;

        public TodoHubTests()
        {
            _contextMock = new Mock<HubCallerContext>();
            _clientsMock = new Mock<IHubCallerClients>();
            _groupsMock = new Mock<IGroupManager>();

            _contextMock.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString());

            _hub = new TodoHub
            {
                Context = _contextMock.Object,
                Clients = _clientsMock.Object,
                Groups = _groupsMock.Object
            };
        }

        #region OnConnectedAsync Tests
        [Fact]
        public async Task OnConnectedAsync_SetsConnectionId()
        {
            var connectionId = Guid.NewGuid().ToString();
            _contextMock.Setup(c => c.ConnectionId).Returns(connectionId);

            await _hub.OnConnectedAsync();

            Assert.Equal(connectionId, _hub.Context.ConnectionId);
        }

        [Fact]
        public async Task OnConnectedAsync_CompletesSuccessfully()
        {
            var exception = await Record.ExceptionAsync(() => _hub.OnConnectedAsync());

            Assert.Null(exception);
        }
        #endregion

        #region OnDisconnectedAsync Tests
        [Fact]
        public async Task OnDisconnectedAsync_WithoutException_CompletesSuccessfully()
        {
            var exception = await Record.ExceptionAsync(() => _hub.OnDisconnectedAsync(null));

            Assert.Null(exception);
        }

        [Fact]
        public async Task OnDisconnectedAsync_WithException_HandlesGracefully()
        {
            var testException = new InvalidOperationException("Test exception");

            var exception = await Record.ExceptionAsync(() => _hub.OnDisconnectedAsync(testException));

            Assert.Null(exception);
        }

        [Fact]
        public async Task OnDisconnectedAsync_WithNetworkError_DoesNotThrow()
        {
            var networkException = new IOException("Network error");

            var exception = await Record.ExceptionAsync(() => _hub.OnDisconnectedAsync(networkException));

            Assert.Null(exception);
        }
        #endregion
    }
}