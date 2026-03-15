using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IRegimenService
    {
        Task<Regimen?> GetRegimenByIdAsync(int id);
        Task<IEnumerable<Regimen>> GetAllRegimenesAsync();
        Task<Regimen> CreateRegimenAsync(Regimen regimen);
        Task<Regimen> UpdateRegimenAsync(Regimen regimen);
        Task<bool> DeleteRegimenAsync(int id);
    }
}
