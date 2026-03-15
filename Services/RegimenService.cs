using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class RegimenService : IRegimenService
    {
        private readonly IRegimenRepository _regimenRepository;

        public RegimenService(IRegimenRepository regimenRepository)
        {
            _regimenRepository = regimenRepository;
        }

        public async Task<Regimen?> GetRegimenByIdAsync(int id)
        {
            return await _regimenRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Regimen>> GetAllRegimenesAsync()
        {
            return await _regimenRepository.GetAllAsync();
        }

        public async Task<Regimen> CreateRegimenAsync(Regimen regimen)
        {
            return await _regimenRepository.AddAsync(regimen);
        }

        public async Task<Regimen> UpdateRegimenAsync(Regimen regimen)
        {
            return await _regimenRepository.UpdateAsync(regimen);
        }

        public async Task<bool> DeleteRegimenAsync(int id)
        {
            return await _regimenRepository.DeleteAsync(id);
        }
    }
}
