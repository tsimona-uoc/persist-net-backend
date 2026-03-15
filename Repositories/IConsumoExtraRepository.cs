using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IConsumoExtraRepository
    {
        Task<ConsumoExtra?> GetByIdAsync(int id);
        Task<IEnumerable<ConsumoExtra>> GetAllAsync();
        Task<IEnumerable<ConsumoExtra>> GetByEstanciaIdAsync(int estanciaId);
        Task<ConsumoExtra> AddAsync(ConsumoExtra consumoExtra);
        Task<ConsumoExtra> UpdateAsync(ConsumoExtra consumoExtra);
        Task<bool> DeleteAsync(int id);
    }
}
