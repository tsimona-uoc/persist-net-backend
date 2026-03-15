using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IMetodoPagoRepository
    {
        Task<MetodoPago?> GetByIdAsync(int id);
        Task<IEnumerable<MetodoPago>> GetAllAsync();
        Task<MetodoPago> AddAsync(MetodoPago metodoPago);
        Task<MetodoPago> UpdateAsync(MetodoPago metodoPago);
        Task<bool> DeleteAsync(int id);
    }
}
