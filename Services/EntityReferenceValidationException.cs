namespace persist_net_backend.Services
{
    public class EntityReferenceValidationException : Exception
    {
        public EntityReferenceValidationException(string message) : base(message)
        {
        }
    }
}