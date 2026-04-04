using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IEntityReferenceValidator _entityReferenceValidator;

        public ReservaService(IReservaRepository reservaRepository, IEntityReferenceValidator entityReferenceValidator)
        {
            _reservaRepository = reservaRepository;
            _entityReferenceValidator = entityReferenceValidator;
        }

        public async Task<Reserva?> GetReservaByIdAsync(int id)
        {
            return await _reservaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Reserva>> GetAllReservasAsync()
        {
            return await _reservaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Reserva>> GetReservasByClienteAsync(int clienteId)
        {
            return await _reservaRepository.GetByClienteIdAsync(clienteId);
        }

        public async Task<IEnumerable<Reserva>> GetReservasByHabitacionAsync(int habitacionId)
        {
            return await _reservaRepository.GetByHabitacionIdAsync(habitacionId);
        }

        public async Task<Reserva> CreateReservaAsync(Reserva reserva)
        {
            await _entityReferenceValidator.EnsureExistsAsync<Cliente>(reserva.ClienteId, "Cliente");
            await _entityReferenceValidator.EnsureExistsAsync<Habitacion>(reserva.HabitacionId, "Habitacion");
            await _entityReferenceValidator.EnsureExistsAsync<Regimen>(reserva.RegimenId, "Regimen");
            await _entityReferenceValidator.EnsureExistsAsync<EstadoReserva>(reserva.EstadoReservaId, "Estado de reserva");

            return await _reservaRepository.AddAsync(reserva);
        }

        public async Task<Reserva> UpdateReservaAsync(Reserva reserva)
        {
            return await _reservaRepository.UpdateAsync(reserva);
        }

        public async Task<bool> DeleteReservaAsync(int id)
        {
            return await _reservaRepository.DeleteAsync(id);
        }
    }
}
