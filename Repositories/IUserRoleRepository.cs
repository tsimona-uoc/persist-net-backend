using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole?> GetById(int id);
        Task<UserRole?> GetByCode(string code);
        Task<IEnumerable<UserRole>> GetAll();
        Task<UserRole> Add(UserRole userRole);
        Task Update(UserRole userRole);
        Task Delete(int id);
    }
}
