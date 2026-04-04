using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class EstanciaService : IEstanciaService
    {
        private readonly IEstanciaRepository _estanciaRepository;
        private readonly IEntityReferenceValidator _entityReferenceValidator;

        public EstanciaService(IEstanciaRepository estanciaRepository, IEntityReferenceValidator entityReferenceValidator)
        {
            _estanciaRepository = estanciaRepository;
            _entityReferenceValidator = entityReferenceValidator;
        }

        public async Task<Estancia?> GetEstanciaByIdAsync(int id)
        {
            return await _estanciaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Estancia>> GetAllEstanciasAsync()
        {
            return await _estanciaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Estancia>> GetEstanciasByReservaAsync(int reservaId)
        {
            return await _estanciaRepository.GetByReservaIdAsync(reservaId);
        }

        public async Task<Estancia> CreateEstanciaAsync(Estancia estancia)
        {
            await _entityReferenceValidator.EnsureExistsAsync<Reserva>(estancia.ReservaId, "Reserva");
            await _entityReferenceValidator.EnsureExistsAsync<EstadoEstancia>(estancia.EstadoEstanciaId, "Estado de estancia");

            return await _estanciaRepository.AddAsync(estancia);
        }

        public async Task<Estancia> UpdateEstanciaAsync(Estancia estancia)
        {
            return await _estanciaRepository.UpdateAsync(estancia);
        }

        public async Task<bool> DeleteEstanciaAsync(int id)
        {
            return await _estanciaRepository.DeleteAsync(id);
        }
    }
}
