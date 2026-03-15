using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class TarifaService : ITarifaService
    {
        private readonly ITarifaRepository _tarifaRepository;

        public TarifaService(ITarifaRepository tarifaRepository)
        {
            _tarifaRepository = tarifaRepository;
        }

        public async Task<Tarifa?> GetTarifaByIdAsync(int id)
        {
            return await _tarifaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Tarifa>> GetAllTarifasAsync()
        {
            return await _tarifaRepository.GetAllAsync();
        }

        public async Task<Tarifa> CreateTarifaAsync(Tarifa tarifa)
        {
            return await _tarifaRepository.AddAsync(tarifa);
        }

        public async Task<Tarifa> UpdateTarifaAsync(Tarifa tarifa)
        {
            return await _tarifaRepository.UpdateAsync(tarifa);
        }

        public async Task<bool> DeleteTarifaAsync(int id)
        {
            return await _tarifaRepository.DeleteAsync(id);
        }
    }
}
