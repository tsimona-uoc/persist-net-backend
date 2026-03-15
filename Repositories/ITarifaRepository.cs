using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface ITarifaRepository
    {
        Task<Tarifa?> GetByIdAsync(int id);
        Task<IEnumerable<Tarifa>> GetAllAsync();
        Task<Tarifa> AddAsync(Tarifa tarifa);
        Task<Tarifa> UpdateAsync(Tarifa tarifa);
        Task<bool> DeleteAsync(int id);
    }
}
