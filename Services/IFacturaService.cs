using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IFacturaService
    {
        Task<Factura?> GetFacturaByIdAsync(int id);
        Task<IEnumerable<Factura>> GetAllFacturasAsync();
        Task<IEnumerable<Factura>> GetFacturasByClienteAsync(int clienteId);
        Task<IEnumerable<Factura>> GetFacturasByEstanciaAsync(int estanciaId);
        Task<Factura> CreateFacturaAsync(Factura factura);
        Task<Factura> UpdateFacturaAsync(Factura factura);
        Task<bool> DeleteFacturaAsync(int id);
    }
}
