using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IHabitacionService
    {
        Task<Habitacion?> GetHabitacionByIdAsync(int id);
        Task<IEnumerable<Habitacion>> GetAllHabitacionesAsync();
        Task<IEnumerable<Habitacion>> GetHabitacionesByHotelAsync(int hotelId);
        Task<Habitacion> CreateHabitacionAsync(Habitacion habitacion);
        Task<Habitacion> UpdateHabitacionAsync(Habitacion habitacion);
        Task<bool> DeleteHabitacionAsync(int id);
    }
}
