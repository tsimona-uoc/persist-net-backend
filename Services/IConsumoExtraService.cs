using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IConsumoExtraService
    {
        Task<ConsumoExtra?> GetConsumoExtraByIdAsync(int id);
        Task<IEnumerable<ConsumoExtra>> GetAllConsumosExtraAsync();
        Task<IEnumerable<ConsumoExtra>> GetConsumoExtrasByEstanciaAsync(int estanciaId);
        Task<ConsumoExtra> CreateConsumoExtraAsync(ConsumoExtra consumoExtra);
        Task<ConsumoExtra> UpdateConsumoExtraAsync(ConsumoExtra consumoExtra);
        Task<bool> DeleteConsumoExtraAsync(int id);
    }
}
