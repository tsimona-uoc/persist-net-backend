using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IClienteService
    {
        Task<Cliente?> GetClienteByIdAsync(int id);
        Task<IEnumerable<Cliente>> GetAllClientesAsync();
        Task<Cliente> CreateClienteAsync(Cliente cliente);
        Task<Cliente> UpdateClienteAsync(Cliente cliente);
        Task<bool> DeleteClienteAsync(int id);
    }
}
