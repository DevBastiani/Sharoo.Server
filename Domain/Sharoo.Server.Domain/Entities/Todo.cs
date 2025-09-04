namespace Sharoo.Server.Domain.Entities
{
    public class Todo : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDone { get; set; } = false;

        public void MarkAsDone()
        {
            IsDone = true;
            CompletedAt = DateTime.Now;
        }

        public void MarkAsNotDone()
        {
            IsDone = false;
            CompletedAt = null;
        }
    }
}
