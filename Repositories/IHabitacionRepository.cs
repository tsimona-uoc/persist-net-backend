using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IHabitacionRepository
    {
        Task<Habitacion?> GetByIdAsync(int id);
        Task<IEnumerable<Habitacion>> GetAllAsync();
        Task<IEnumerable<Habitacion>> GetByHotelIdAsync(int hotelId);
        Task<Habitacion> AddAsync(Habitacion habitacion);
        Task<Habitacion> UpdateAsync(Habitacion habitacion);
        Task<bool> DeleteAsync(int id);
    }
}
