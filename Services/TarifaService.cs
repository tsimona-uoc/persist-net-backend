using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class TarifaService : ITarifaService
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly IEntityReferenceValidator _entityReferenceValidator;

        public TarifaService(ITarifaRepository tarifaRepository, IEntityReferenceValidator entityReferenceValidator)
        {
            _tarifaRepository = tarifaRepository;
            _entityReferenceValidator = entityReferenceValidator;
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
            await _entityReferenceValidator.EnsureExistsAsync<Temporada>(tarifa.TemporadaId, "Temporada");

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
