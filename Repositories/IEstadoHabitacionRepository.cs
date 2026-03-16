using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IEstadoHabitacionRepository
    {
        Task<EstadoHabitacion?> GetByIdAsync(int id);
        Task<IEnumerable<EstadoHabitacion>> GetAllAsync();
        Task<EstadoHabitacion> AddAsync(EstadoHabitacion estadoHabitacion);
        Task<EstadoHabitacion> UpdateAsync(EstadoHabitacion estadoHabitacion);
        Task<bool> DeleteAsync(int id);
    }
}
