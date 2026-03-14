namespace persist_net_backend.Services
{
    public interface IAuthService
    {
        Task<(bool success, string token)> LoginAsync(string email, string password);
        Task<(bool success, string token)> RegisterAsync(string name, string surname, string email, string password);
    }
}