namespace Sharoo.Server.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message = "Usuário não foi encontrado.")
            : base(message) { }
    }
}