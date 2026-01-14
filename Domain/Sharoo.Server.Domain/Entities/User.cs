namespace Sharoo.Server.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<string> Role { get; set; } = new List<string> { "COMMON" };

        public ICollection<Todo>? Todos { get; set; }
    }
}
