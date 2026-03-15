using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface ITipoHabitacionRepository
    {
        Task<TipoHabitacion?> GetByIdAsync(int id);
        Task<IEnumerable<TipoHabitacion>> GetAllAsync();
        Task<TipoHabitacion> AddAsync(TipoHabitacion tipoHabitacion);
        Task<TipoHabitacion> UpdateAsync(TipoHabitacion tipoHabitacion);
        Task<bool> DeleteAsync(int id);
    }
}
