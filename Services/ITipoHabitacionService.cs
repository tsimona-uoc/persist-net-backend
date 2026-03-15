using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface ITipoHabitacionService
    {
        Task<TipoHabitacion?> GetTipoHabitacionByIdAsync(int id);
        Task<IEnumerable<TipoHabitacion>> GetAllTiposHabitacionAsync();
        Task<TipoHabitacion> CreateTipoHabitacionAsync(TipoHabitacion tipoHabitacion);
        Task<TipoHabitacion> UpdateTipoHabitacionAsync(TipoHabitacion tipoHabitacion);
        Task<bool> DeleteTipoHabitacionAsync(int id);
    }
}
