using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class EstadoEstanciaService : IEstadoEstanciaService
    {
        private readonly IEstadoEstanciaRepository _estadoEstanciaRepository;

        public EstadoEstanciaService(IEstadoEstanciaRepository estadoEstanciaRepository)
        {
            _estadoEstanciaRepository = estadoEstanciaRepository;
        }

        public async Task<EstadoEstancia?> GetEstadoEstanciaByIdAsync(int id)
        {
            return await _estadoEstanciaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EstadoEstancia>> GetAllEstadosEstanciaAsync()
        {
            return await _estadoEstanciaRepository.GetAllAsync();
        }

        public async Task<EstadoEstancia> CreateEstadoEstanciaAsync(EstadoEstancia estadoEstancia)
        {
            return await _estadoEstanciaRepository.AddAsync(estadoEstancia);
        }

        public async Task<EstadoEstancia> UpdateEstadoEstanciaAsync(EstadoEstancia estadoEstancia)
        {
            return await _estadoEstanciaRepository.UpdateAsync(estadoEstancia);
        }

        public async Task<bool> DeleteEstadoEstanciaAsync(int id)
        {
            return await _estadoEstanciaRepository.DeleteAsync(id);
        }
    }
}
