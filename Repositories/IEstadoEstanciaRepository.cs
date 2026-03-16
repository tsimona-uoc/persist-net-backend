using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IEstadoEstanciaRepository
    {
        Task<EstadoEstancia?> GetByIdAsync(int id);
        Task<IEnumerable<EstadoEstancia>> GetAllAsync();
        Task<EstadoEstancia> AddAsync(EstadoEstancia estadoEstancia);
        Task<EstadoEstancia> UpdateAsync(EstadoEstancia estadoEstancia);
        Task<bool> DeleteAsync(int id);
    }
}
