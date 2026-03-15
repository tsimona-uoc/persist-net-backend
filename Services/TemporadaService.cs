using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class TemporadaService : ITemporadaService
    {
        private readonly ITemporadaRepository _temporadaRepository;

        public TemporadaService(ITemporadaRepository temporadaRepository)
        {
            _temporadaRepository = temporadaRepository;
        }

        public async Task<Temporada?> GetTemporadaByIdAsync(int id)
        {
            return await _temporadaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Temporada>> GetAllTemporadasAsync()
        {
            return await _temporadaRepository.GetAllAsync();
        }

        public async Task<Temporada> CreateTemporadaAsync(Temporada temporada)
        {
            return await _temporadaRepository.AddAsync(temporada);
        }

        public async Task<Temporada> UpdateTemporadaAsync(Temporada temporada)
        {
            return await _temporadaRepository.UpdateAsync(temporada);
        }

        public async Task<bool> DeleteTemporadaAsync(int id)
        {
            return await _temporadaRepository.DeleteAsync(id);
        }
    }
}
