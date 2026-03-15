using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IReservaRepository
    {
        Task<Reserva?> GetByIdAsync(int id);
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task<IEnumerable<Reserva>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Reserva>> GetByHabitacionIdAsync(int habitacionId);
        Task<Reserva> AddAsync(Reserva reserva);
        Task<Reserva> UpdateAsync(Reserva reserva);
        Task<bool> DeleteAsync(int id);
    }
}
