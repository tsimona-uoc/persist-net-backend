using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class EstadoReservaService : IEstadoReservaService
    {
        private readonly IEstadoReservaRepository _estadoReservaRepository;

        public EstadoReservaService(IEstadoReservaRepository estadoReservaRepository)
        {
            _estadoReservaRepository = estadoReservaRepository;
        }

        public async Task<EstadoReserva?> GetEstadoReservaByIdAsync(int id)
        {
            return await _estadoReservaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EstadoReserva>> GetAllEstadosReservaAsync()
        {
            return await _estadoReservaRepository.GetAllAsync();
        }

        public async Task<EstadoReserva> CreateEstadoReservaAsync(EstadoReserva estadoReserva)
        {
            return await _estadoReservaRepository.AddAsync(estadoReserva);
        }

        public async Task<EstadoReserva> UpdateEstadoReservaAsync(EstadoReserva estadoReserva)
        {
            return await _estadoReservaRepository.UpdateAsync(estadoReserva);
        }

        public async Task<bool> DeleteEstadoReservaAsync(int id)
        {
            return await _estadoReservaRepository.DeleteAsync(id);
        }
    }
}
