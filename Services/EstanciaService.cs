using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class EstanciaService : IEstanciaService
    {
        private static readonly string[] AvailableRoomStatusNames = ["DISPONIBLE", "LIBRE"];
        private static readonly string[] OccupiedRoomStatusNames = ["OCUPADO", "OCUPADA"];

        private readonly IEstanciaRepository _estanciaRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IHabitacionRepository _habitacionRepository;
        private readonly IEstadoHabitacionRepository _estadoHabitacionRepository;
        private readonly IEntityReferenceValidator _entityReferenceValidator;

        public EstanciaService(
            IEstanciaRepository estanciaRepository,
            IReservaRepository reservaRepository,
            IHabitacionRepository habitacionRepository,
            IEstadoHabitacionRepository estadoHabitacionRepository,
            IEntityReferenceValidator entityReferenceValidator)
        {
            _estanciaRepository = estanciaRepository;
            _reservaRepository = reservaRepository;
            _habitacionRepository = habitacionRepository;
            _estadoHabitacionRepository = estadoHabitacionRepository;
            _entityReferenceValidator = entityReferenceValidator;
        }

        public async Task<Estancia?> GetEstanciaByIdAsync(int id)
        {
            return await _estanciaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Estancia>> GetAllEstanciasAsync()
        {
            return await _estanciaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Estancia>> GetEstanciasByReservaAsync(int reservaId)
        {
            return await _estanciaRepository.GetByReservaIdAsync(reservaId);
        }

        public async Task<Estancia> CreateEstanciaAsync(Estancia estancia)
        {
            await _entityReferenceValidator.EnsureExistsAsync<Reserva>(estancia.ReservaId, "Reserva");
            await _entityReferenceValidator.EnsureExistsAsync<EstadoEstancia>(estancia.EstadoEstanciaId, "Estado de estancia");

            var createdEstancia = await _estanciaRepository.AddAsync(estancia);
            await SyncHabitacionStatusForReservaAsync(createdEstancia.ReservaId);

            return createdEstancia;
        }

        public async Task<Estancia> UpdateEstanciaAsync(Estancia estancia)
        {
            var persistedEstancia = await _estanciaRepository.GetByIdNoTrackingAsync(estancia.Id)
                ?? throw new InvalidOperationException("Estancia not found.");

            var previousReservaId = persistedEstancia.ReservaId;

            await _entityReferenceValidator.EnsureExistsAsync<Reserva>(estancia.ReservaId, "Reserva");
            await _entityReferenceValidator.EnsureExistsAsync<EstadoEstancia>(estancia.EstadoEstanciaId, "Estado de estancia");

            var updatedEstancia = await _estanciaRepository.UpdateAsync(estancia);

            if (previousReservaId != updatedEstancia.ReservaId)
            {
                await SyncHabitacionStatusForReservaAsync(previousReservaId);
            }

            await SyncHabitacionStatusForReservaAsync(updatedEstancia.ReservaId);

            return updatedEstancia;
        }

        public async Task<bool> DeleteEstanciaAsync(int id)
        {
            var persistedEstancia = await _estanciaRepository.GetByIdAsync(id);
            if (persistedEstancia == null)
            {
                return false;
            }

            var deleted = await _estanciaRepository.DeleteAsync(id);
            if (deleted)
            {
                await SyncHabitacionStatusForReservaAsync(persistedEstancia.ReservaId);
            }

            return deleted;
        }

        private async Task SyncHabitacionStatusForReservaAsync(int reservaId)
        {
            var reserva = await _reservaRepository.GetByIdAsync(reservaId)
                ?? throw new InvalidOperationException("Reserva not found.");

            var habitacion = await _habitacionRepository.GetByIdAsync(reserva.HabitacionId)
                ?? throw new InvalidOperationException("Habitacion not found.");

            var estadosHabitacion = (await _estadoHabitacionRepository.GetAllAsync()).ToList();
            var occupiedStatus = FindRoomStatus(estadosHabitacion, OccupiedRoomStatusNames)
                ?? throw new InvalidOperationException("Occupied room status is not configured.");
            var availableStatus = FindRoomStatus(estadosHabitacion, AvailableRoomStatusNames)
                ?? throw new InvalidOperationException("Available room status is not configured.");

            var hasActiveEstancia = await _estanciaRepository.HasActiveEstanciaByHabitacionIdAsync(habitacion.Id);
            var currentStatusName = habitacion.EstadoHabitacion?.Nombre?.Trim().ToUpperInvariant() ?? string.Empty;

            if (hasActiveEstancia)
            {
                if (habitacion.EstadoHabitacionId != occupiedStatus.Id)
                {
                    habitacion.EstadoHabitacionId = occupiedStatus.Id;
                    habitacion.LastModifiedBy = "system";
                    habitacion.LastModifiedAt = DateTime.Now;
                    await _habitacionRepository.UpdateAsync(habitacion);
                }

                return;
            }

            if (OccupiedRoomStatusNames.Contains(currentStatusName))
            {
                habitacion.EstadoHabitacionId = availableStatus.Id;
                habitacion.LastModifiedBy = "system";
                habitacion.LastModifiedAt = DateTime.Now;
                await _habitacionRepository.UpdateAsync(habitacion);
            }
        }

        private static EstadoHabitacion? FindRoomStatus(IEnumerable<EstadoHabitacion> states, IEnumerable<string> acceptedNames)
        {
            var accepted = acceptedNames.ToHashSet(StringComparer.OrdinalIgnoreCase);

            return states.FirstOrDefault(state => accepted.Contains(state.Nombre.Trim().ToUpperInvariant()));
        }
    }
}
