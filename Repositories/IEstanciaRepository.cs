using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IEstanciaRepository
    {
        Task<Estancia?> GetByIdAsync(int id);
        Task<Estancia?> GetByIdNoTrackingAsync(int id);
        Task<IEnumerable<Estancia>> GetAllAsync();
        Task<IEnumerable<Estancia>> GetByReservaIdAsync(int reservaId);
        Task<bool> HasActiveEstanciaByHabitacionIdAsync(int habitacionId, int? excludeEstanciaId = null);
        Task<Estancia> AddAsync(Estancia estancia);
        Task<Estancia> UpdateAsync(Estancia estancia);
        Task<bool> DeleteAsync(int id);
    }
}
