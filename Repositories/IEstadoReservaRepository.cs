using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IEstadoReservaRepository
    {
        Task<EstadoReserva?> GetByIdAsync(int id);
        Task<IEnumerable<EstadoReserva>> GetAllAsync();
        Task<EstadoReserva> AddAsync(EstadoReserva estadoReserva);
        Task<EstadoReserva> UpdateAsync(EstadoReserva estadoReserva);
        Task<bool> DeleteAsync(int id);
    }
}
