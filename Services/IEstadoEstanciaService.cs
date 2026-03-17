using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IEstadoEstanciaService
    {
        Task<EstadoEstancia?> GetEstadoEstanciaByIdAsync(int id);
        Task<IEnumerable<EstadoEstancia>> GetAllEstadosEstanciaAsync();
        Task<EstadoEstancia> CreateEstadoEstanciaAsync(EstadoEstancia estadoEstancia);
        Task<EstadoEstancia> UpdateEstadoEstanciaAsync(EstadoEstancia estadoEstancia);
        Task<bool> DeleteEstadoEstanciaAsync(int id);
    }
}
