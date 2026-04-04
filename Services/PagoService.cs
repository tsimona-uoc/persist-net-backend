using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly IEntityReferenceValidator _entityReferenceValidator;

        public PagoService(IPagoRepository pagoRepository, IEntityReferenceValidator entityReferenceValidator)
        {
            _pagoRepository = pagoRepository;
            _entityReferenceValidator = entityReferenceValidator;
        }

        public async Task<Pago?> GetPagoByIdAsync(int id)
        {
            return await _pagoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Pago>> GetAllPagosAsync()
        {
            return await _pagoRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Pago>> GetPagosByFacturaAsync(int facturaId)
        {
            return await _pagoRepository.GetByFacturaIdAsync(facturaId);
        }

        public async Task<Pago> CreatePagoAsync(Pago pago)
        {
            await _entityReferenceValidator.EnsureExistsAsync<Factura>(pago.FacturaId, "Factura");
            await _entityReferenceValidator.EnsureExistsAsync<MetodoPago>(pago.MetodoPagoId, "Metodo de pago");

            return await _pagoRepository.AddAsync(pago);
        }

        public async Task<Pago> UpdatePagoAsync(Pago pago)
        {
            return await _pagoRepository.UpdateAsync(pago);
        }

        public async Task<bool> DeletePagoAsync(int id)
        {
            return await _pagoRepository.DeleteAsync(id);
        }
    }
}
