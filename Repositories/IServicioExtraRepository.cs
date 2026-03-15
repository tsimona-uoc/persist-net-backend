using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IServicioExtraRepository
    {
        Task<ServicioExtra?> GetByIdAsync(int id);
        Task<IEnumerable<ServicioExtra>> GetAllAsync();
        Task<ServicioExtra> AddAsync(ServicioExtra servicioExtra);
        Task<ServicioExtra> UpdateAsync(ServicioExtra servicioExtra);
        Task<bool> DeleteAsync(int id);
    }
}
