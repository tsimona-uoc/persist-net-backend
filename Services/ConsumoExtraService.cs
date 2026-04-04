using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class ConsumoExtraService : IConsumoExtraService
    {
        private readonly IConsumoExtraRepository _consumoExtraRepository;
        private readonly IEntityReferenceValidator _entityReferenceValidator;

        public ConsumoExtraService(IConsumoExtraRepository consumoExtraRepository, IEntityReferenceValidator entityReferenceValidator)
        {
            _consumoExtraRepository = consumoExtraRepository;
            _entityReferenceValidator = entityReferenceValidator;
        }

        public async Task<ConsumoExtra?> GetConsumoExtraByIdAsync(int id)
        {
            return await _consumoExtraRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ConsumoExtra>> GetAllConsumosExtraAsync()
        {
            return await _consumoExtraRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ConsumoExtra>> GetConsumoExtrasByEstanciaAsync(int estanciaId)
        {
            return await _consumoExtraRepository.GetByEstanciaIdAsync(estanciaId);
        }

        public async Task<ConsumoExtra> CreateConsumoExtraAsync(ConsumoExtra consumoExtra)
        {
            await _entityReferenceValidator.EnsureExistsAsync<Estancia>(consumoExtra.EstanciaId, "Estancia");
            await _entityReferenceValidator.EnsureExistsAsync<ServicioExtra>(consumoExtra.ServicioExtraId, "Servicio extra");

            return await _consumoExtraRepository.AddAsync(consumoExtra);
        }

        public async Task<ConsumoExtra> UpdateConsumoExtraAsync(ConsumoExtra consumoExtra)
        {
            return await _consumoExtraRepository.UpdateAsync(consumoExtra);
        }

        public async Task<bool> DeleteConsumoExtraAsync(int id)
        {
            return await _consumoExtraRepository.DeleteAsync(id);
        }
    }
}
