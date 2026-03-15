using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IPagoService
    {
        Task<Pago?> GetPagoByIdAsync(int id);
        Task<IEnumerable<Pago>> GetAllPagosAsync();
        Task<IEnumerable<Pago>> GetPagosByFacturaAsync(int facturaId);
        Task<Pago> CreatePagoAsync(Pago pago);
        Task<Pago> UpdatePagoAsync(Pago pago);
        Task<bool> DeletePagoAsync(int id);
    }
}
