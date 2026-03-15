using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IFacturaRepository
    {
        Task<Factura?> GetByIdAsync(int id);
        Task<IEnumerable<Factura>> GetAllAsync();
        Task<IEnumerable<Factura>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Factura>> GetByEstanciaIdAsync(int estanciaId);
        Task<Factura> AddAsync(Factura factura);
        Task<Factura> UpdateAsync(Factura factura);
        Task<bool> DeleteAsync(int id);
    }
}
