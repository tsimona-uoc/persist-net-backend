using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface ITemporadaRepository
    {
        Task<Temporada?> GetByIdAsync(int id);
        Task<IEnumerable<Temporada>> GetAllAsync();
        Task<Temporada> AddAsync(Temporada temporada);
        Task<Temporada> UpdateAsync(Temporada temporada);
        Task<bool> DeleteAsync(int id);
    }
}
