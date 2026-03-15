using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IMetodoPagoService
    {
        Task<MetodoPago?> GetMetodoPagoByIdAsync(int id);
        Task<IEnumerable<MetodoPago>> GetAllMetodosPagoAsync();
        Task<MetodoPago> CreateMetodoPagoAsync(MetodoPago metodoPago);
        Task<MetodoPago> UpdateMetodoPagoAsync(MetodoPago metodoPago);
        Task<bool> DeleteMetodoPagoAsync(int id);
    }
}
