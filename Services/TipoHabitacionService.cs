using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class TipoHabitacionService : ITipoHabitacionService
    {
        private readonly ITipoHabitacionRepository _tipoHabitacionRepository;

        public TipoHabitacionService(ITipoHabitacionRepository tipoHabitacionRepository)
        {
            _tipoHabitacionRepository = tipoHabitacionRepository;
        }

        public async Task<TipoHabitacion?> GetTipoHabitacionByIdAsync(int id)
        {
            return await _tipoHabitacionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TipoHabitacion>> GetAllTiposHabitacionAsync()
        {
            return await _tipoHabitacionRepository.GetAllAsync();
        }

        public async Task<TipoHabitacion> CreateTipoHabitacionAsync(TipoHabitacion tipoHabitacion)
        {
            return await _tipoHabitacionRepository.AddAsync(tipoHabitacion);
        }

        public async Task<TipoHabitacion> UpdateTipoHabitacionAsync(TipoHabitacion tipoHabitacion)
        {
            return await _tipoHabitacionRepository.UpdateAsync(tipoHabitacion);
        }

        public async Task<bool> DeleteTipoHabitacionAsync(int id)
        {
            return await _tipoHabitacionRepository.DeleteAsync(id);
        }
    }
}
