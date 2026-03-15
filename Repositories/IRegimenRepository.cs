using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IRegimenRepository
    {
        Task<Regimen?> GetByIdAsync(int id);
        Task<IEnumerable<Regimen>> GetAllAsync();
        Task<Regimen> AddAsync(Regimen regimen);
        Task<Regimen> UpdateAsync(Regimen regimen);
        Task<bool> DeleteAsync(int id);
    }
}
