using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface ITemporadaService
    {
        Task<Temporada?> GetTemporadaByIdAsync(int id);
        Task<IEnumerable<Temporada>> GetAllTemporadasAsync();
        Task<Temporada> CreateTemporadaAsync(Temporada temporada);
        Task<Temporada> UpdateTemporadaAsync(Temporada temporada);
        Task<bool> DeleteTemporadaAsync(int id);
    }
}
