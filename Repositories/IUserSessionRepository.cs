using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IUserSessionRepository
    {
        public Task<UserSession> UpdateSessionAsync(Guid userId, string token, string address);
    }
}