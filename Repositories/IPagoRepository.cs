using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IPagoRepository
    {
        Task<Pago?> GetByIdAsync(int id);
        Task<IEnumerable<Pago>> GetAllAsync();
        Task<IEnumerable<Pago>> GetByFacturaIdAsync(int facturaId);
        Task<Pago> AddAsync(Pago pago);
        Task<Pago> UpdateAsync(Pago pago);
        Task<bool> DeleteAsync(int id);
    }
}
