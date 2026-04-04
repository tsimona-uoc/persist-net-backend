namespace persist_net_backend.Services
{
    public interface IEntityReferenceValidator
    {
        Task EnsureExistsAsync<TEntity>(int id, string entityName) where TEntity : class;
    }
}