using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class HabitacionService : IHabitacionService
    {
        private readonly IHabitacionRepository _habitacionRepository;
        private readonly IEntityReferenceValidator _entityReferenceValidator;

        public HabitacionService(IHabitacionRepository habitacionRepository, IEntityReferenceValidator entityReferenceValidator)
        {
            _habitacionRepository = habitacionRepository;
            _entityReferenceValidator = entityReferenceValidator;
        }

        public async Task<Habitacion?> GetHabitacionByIdAsync(int id)
        {
            return await _habitacionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Habitacion>> GetAllHabitacionesAsync()
        {
            return await _habitacionRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Habitacion>> GetHabitacionesByHotelAsync(int hotelId)
        {
            return await _habitacionRepository.GetByHotelIdAsync(hotelId);
        }

        public async Task<Habitacion> CreateHabitacionAsync(Habitacion habitacion)
        {
            await _entityReferenceValidator.EnsureExistsAsync<Hotel>(habitacion.HotelId, "Hotel");
            await _entityReferenceValidator.EnsureExistsAsync<TipoHabitacion>(habitacion.TipoHabitacionId, "Tipo de habitacion");
            await _entityReferenceValidator.EnsureExistsAsync<EstadoHabitacion>(habitacion.EstadoHabitacionId, "Estado de habitacion");

            return await _habitacionRepository.AddAsync(habitacion);
        }

        public async Task<Habitacion> UpdateHabitacionAsync(Habitacion habitacion)
        {
            return await _habitacionRepository.UpdateAsync(habitacion);
        }

        public async Task<bool> DeleteHabitacionAsync(int id)
        {
            return await _habitacionRepository.DeleteAsync(id);
        }
    }
}
