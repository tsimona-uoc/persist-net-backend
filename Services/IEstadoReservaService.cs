using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IEstadoReservaService
    {
        Task<EstadoReserva?> GetEstadoReservaByIdAsync(int id);
        Task<IEnumerable<EstadoReserva>> GetAllEstadosReservaAsync();
        Task<EstadoReserva> CreateEstadoReservaAsync(EstadoReserva estadoReserva);
        Task<EstadoReserva> UpdateEstadoReservaAsync(EstadoReserva estadoReserva);
        Task<bool> DeleteEstadoReservaAsync(int id);
    }
}
