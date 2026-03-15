using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface ITarifaService
    {
        Task<Tarifa?> GetTarifaByIdAsync(int id);
        Task<IEnumerable<Tarifa>> GetAllTarifasAsync();
        Task<Tarifa> CreateTarifaAsync(Tarifa tarifa);
        Task<Tarifa> UpdateTarifaAsync(Tarifa tarifa);
        Task<bool> DeleteTarifaAsync(int id);
    }
}
