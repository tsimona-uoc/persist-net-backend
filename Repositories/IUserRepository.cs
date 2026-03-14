using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IUserRepository
    {
        Task<User?> findById(Guid id);
        Task<User?> findByEmail(string email);
        Task<User> Add(User user);
    }
}