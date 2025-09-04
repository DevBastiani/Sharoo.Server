namespace Sharoo.Server.Application.DTOs.Todos.Request
{
    public class TodoChangeStatusRequest
    {
        public Guid TaskId { get; set; }
        public bool IsDone { get; set; }
    }
}
