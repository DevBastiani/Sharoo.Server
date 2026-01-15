using Microsoft.AspNetCore.SignalR;
using Sharoo.Server.API.Hubs;
using Sharoo.Server.Application.Services.Notifications;

namespace Sharoo.Server.API.Services
{
    public class TodoNotificationService : ITodoNotificationService
    {
        private readonly IHubContext<TodoHub> _hubContext;

        public TodoNotificationService(IHubContext<TodoHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyTodoCreatedAsync(Guid id, string name, bool isDone, DateTime createdAt)
        {
            await _hubContext.Clients.All.SendAsync("TodoCreated", new
            {
                id,
                name,
                isDone,
                createdAt
            });
        }

        public async Task NotifyTodoStatusChangedAsync(Guid id, bool isDone)
        {
            await _hubContext.Clients.All.SendAsync("TodoStatusChanged", new
            {
                id,
                isDone
            });
        }

        public async Task NotifyTodoDeletedAsync(Guid id)
        {
            await _hubContext.Clients.All.SendAsync("TodoDeleted", id);
        }
    }
}
