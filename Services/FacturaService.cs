using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IEntityReferenceValidator _entityReferenceValidator;

        public FacturaService(IFacturaRepository facturaRepository, IEntityReferenceValidator entityReferenceValidator)
        {
            _facturaRepository = facturaRepository;
            _entityReferenceValidator = entityReferenceValidator;
        }

        public async Task<Factura?> GetFacturaByIdAsync(int id)
        {
            return await _facturaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Factura>> GetAllFacturasAsync()
        {
            return await _facturaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Factura>> GetFacturasByClienteAsync(int clienteId)
        {
            return await _facturaRepository.GetByClienteIdAsync(clienteId);
        }

        public async Task<IEnumerable<Factura>> GetFacturasByEstanciaAsync(int estanciaId)
        {
            return await _facturaRepository.GetByEstanciaIdAsync(estanciaId);
        }

        public async Task<Factura> CreateFacturaAsync(Factura factura)
        {
            await _entityReferenceValidator.EnsureExistsAsync<Estancia>(factura.EstanciaId, "Estancia");
            await _entityReferenceValidator.EnsureExistsAsync<Cliente>(factura.ClienteId, "Cliente");

            return await _facturaRepository.AddAsync(factura);
        }

        public async Task<Factura> UpdateFacturaAsync(Factura factura)
        {
            return await _facturaRepository.UpdateAsync(factura);
        }

        public async Task<bool> DeleteFacturaAsync(int id)
        {
            return await _facturaRepository.DeleteAsync(id);
        }
    }
}
