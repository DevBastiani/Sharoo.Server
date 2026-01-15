namespace Sharoo.Server.Application.Services.Notifications
{
    public interface ITodoNotificationService
    {
        Task NotifyTodoCreatedAsync(Guid id, string name, bool isDone, DateTime createdAt);
        Task NotifyTodoStatusChangedAsync(Guid id, bool isDone);
        Task NotifyTodoDeletedAsync(Guid id);
    }
}