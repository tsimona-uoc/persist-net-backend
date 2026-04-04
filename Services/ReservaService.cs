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
            ValidateReservationDates(reserva.FechaEntrada, reserva.FechaSalida);

            await _entityReferenceValidator.EnsureExistsAsync<Cliente>(reserva.ClienteId, "Cliente");
            await _entityReferenceValidator.EnsureExistsAsync<Habitacion>(reserva.HabitacionId, "Habitacion");
            await _entityReferenceValidator.EnsureExistsAsync<Regimen>(reserva.RegimenId, "Regimen");
            await _entityReferenceValidator.EnsureExistsAsync<EstadoReserva>(reserva.EstadoReservaId, "Estado de reserva");

            await EnsureHabitacionIsAvailableAsync(reserva.HabitacionId, reserva.FechaEntrada, reserva.FechaSalida);

            return await _reservaRepository.AddAsync(reserva);
        }

        public async Task<Reserva> UpdateReservaAsync(Reserva reserva)
        {
            ValidateReservationDates(reserva.FechaEntrada, reserva.FechaSalida);

            await _entityReferenceValidator.EnsureExistsAsync<Cliente>(reserva.ClienteId, "Cliente");
            await _entityReferenceValidator.EnsureExistsAsync<Habitacion>(reserva.HabitacionId, "Habitacion");
            await _entityReferenceValidator.EnsureExistsAsync<Regimen>(reserva.RegimenId, "Regimen");
            await _entityReferenceValidator.EnsureExistsAsync<EstadoReserva>(reserva.EstadoReservaId, "Estado de reserva");

            await EnsureHabitacionIsAvailableAsync(reserva.HabitacionId, reserva.FechaEntrada, reserva.FechaSalida, reserva.Id);

            return await _reservaRepository.UpdateAsync(reserva);
        }

        public async Task<bool> DeleteReservaAsync(int id)
        {
            return await _reservaRepository.DeleteAsync(id);
        }

        private static void ValidateReservationDates(DateOnly fechaEntrada, DateOnly fechaSalida)
        {
            if (fechaEntrada >= fechaSalida)
            {
                throw new ReservaValidationException("La fecha de entrada debe ser anterior a la fecha de salida.");
            }
        }

        private async Task EnsureHabitacionIsAvailableAsync(int habitacionId, DateOnly fechaEntrada, DateOnly fechaSalida, int? excludeReservaId = null)
        {
            var hasOverlap = await _reservaRepository.HasOverlappingReservationAsync(habitacionId, fechaEntrada, fechaSalida, excludeReservaId);
            if (hasOverlap)
            {
                throw new ReservaValidationException("Ya existe una reserva para esa habitación en el rango de fechas indicado.");
            }
        }
    }
}
