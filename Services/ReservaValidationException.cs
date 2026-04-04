namespace persist_net_backend.Services
{
    public class ReservaValidationException : Exception
    {
        public ReservaValidationException(string message) : base(message)
        {
        }
    }
}