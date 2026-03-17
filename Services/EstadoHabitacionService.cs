using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class EstadoHabitacionService : IEstadoHabitacionService
    {
        private readonly IEstadoHabitacionRepository _estadoHabitacionRepository;

        public EstadoHabitacionService(IEstadoHabitacionRepository estadoHabitacionRepository)
        {
            _estadoHabitacionRepository = estadoHabitacionRepository;
        }

        public async Task<EstadoHabitacion?> GetEstadoHabitacionByIdAsync(int id)
        {
            return await _estadoHabitacionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EstadoHabitacion>> GetAllEstadosHabitacionAsync()
        {
            return await _estadoHabitacionRepository.GetAllAsync();
        }

        public async Task<EstadoHabitacion> CreateEstadoHabitacionAsync(EstadoHabitacion estadoHabitacion)
        {
            return await _estadoHabitacionRepository.AddAsync(estadoHabitacion);
        }

        public async Task<EstadoHabitacion> UpdateEstadoHabitacionAsync(EstadoHabitacion estadoHabitacion)
        {
            return await _estadoHabitacionRepository.UpdateAsync(estadoHabitacion);
        }

        public async Task<bool> DeleteEstadoHabitacionAsync(int id)
        {
            return await _estadoHabitacionRepository.DeleteAsync(id);
        }
    }
}
