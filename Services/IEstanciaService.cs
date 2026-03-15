using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IEstanciaService
    {
        Task<Estancia?> GetEstanciaByIdAsync(int id);
        Task<IEnumerable<Estancia>> GetAllEstanciasAsync();
        Task<IEnumerable<Estancia>> GetEstanciasByReservaAsync(int reservaId);
        Task<Estancia> CreateEstanciaAsync(Estancia estancia);
        Task<Estancia> UpdateEstanciaAsync(Estancia estancia);
        Task<bool> DeleteEstanciaAsync(int id);
    }
}
