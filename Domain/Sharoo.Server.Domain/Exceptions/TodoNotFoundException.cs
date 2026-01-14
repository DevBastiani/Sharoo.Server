namespace Sharoo.Server.Domain.Exceptions
{
    public class TodoNotFoundException : Exception
    {
        public TodoNotFoundException(string message = "TODO não foi encontrado.")
            : base(message) { }
    }
}