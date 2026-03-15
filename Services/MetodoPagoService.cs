using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {
        private readonly IMetodoPagoRepository _metodoPagoRepository;

        public MetodoPagoService(IMetodoPagoRepository metodoPagoRepository)
        {
            _metodoPagoRepository = metodoPagoRepository;
        }

        public async Task<MetodoPago?> GetMetodoPagoByIdAsync(int id)
        {
            return await _metodoPagoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<MetodoPago>> GetAllMetodosPagoAsync()
        {
            return await _metodoPagoRepository.GetAllAsync();
        }

        public async Task<MetodoPago> CreateMetodoPagoAsync(MetodoPago metodoPago)
        {
            return await _metodoPagoRepository.AddAsync(metodoPago);
        }

        public async Task<MetodoPago> UpdateMetodoPagoAsync(MetodoPago metodoPago)
        {
            return await _metodoPagoRepository.UpdateAsync(metodoPago);
        }

        public async Task<bool> DeleteMetodoPagoAsync(int id)
        {
            return await _metodoPagoRepository.DeleteAsync(id);
        }
    }
}
