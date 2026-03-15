using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IServicioExtraService
    {
        Task<ServicioExtra?> GetServicioExtraByIdAsync(int id);
        Task<IEnumerable<ServicioExtra>> GetAllServiciosExtraAsync();
        Task<ServicioExtra> CreateServicioExtraAsync(ServicioExtra servicioExtra);
        Task<ServicioExtra> UpdateServicioExtraAsync(ServicioExtra servicioExtra);
        Task<bool> DeleteServicioExtraAsync(int id);
    }
}
