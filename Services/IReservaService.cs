using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IReservaService
    {
        Task<Reserva?> GetReservaByIdAsync(int id);
        Task<IEnumerable<Reserva>> GetAllReservasAsync();
        Task<IEnumerable<Reserva>> GetReservasByClienteAsync(int clienteId);
        Task<IEnumerable<Reserva>> GetReservasByHabitacionAsync(int habitacionId);
        Task<Reserva> CreateReservaAsync(Reserva reserva);
        Task<Reserva> UpdateReservaAsync(Reserva reserva);
        Task<bool> DeleteReservaAsync(int id);
    }
}
