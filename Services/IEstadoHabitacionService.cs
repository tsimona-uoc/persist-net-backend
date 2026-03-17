using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IEstadoHabitacionService
    {
        Task<EstadoHabitacion?> GetEstadoHabitacionByIdAsync(int id);
        Task<IEnumerable<EstadoHabitacion>> GetAllEstadosHabitacionAsync();
        Task<EstadoHabitacion> CreateEstadoHabitacionAsync(EstadoHabitacion estadoHabitacion);
        Task<EstadoHabitacion> UpdateEstadoHabitacionAsync(EstadoHabitacion estadoHabitacion);
        Task<bool> DeleteEstadoHabitacionAsync(int id);
    }
}
