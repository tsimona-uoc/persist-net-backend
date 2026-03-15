using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class ServicioExtraService : IServicioExtraService
    {
        private readonly IServicioExtraRepository _servicioExtraRepository;

        public ServicioExtraService(IServicioExtraRepository servicioExtraRepository)
        {
            _servicioExtraRepository = servicioExtraRepository;
        }

        public async Task<ServicioExtra?> GetServicioExtraByIdAsync(int id)
        {
            return await _servicioExtraRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ServicioExtra>> GetAllServiciosExtraAsync()
        {
            return await _servicioExtraRepository.GetAllAsync();
        }

        public async Task<ServicioExtra> CreateServicioExtraAsync(ServicioExtra servicioExtra)
        {
            return await _servicioExtraRepository.AddAsync(servicioExtra);
        }

        public async Task<ServicioExtra> UpdateServicioExtraAsync(ServicioExtra servicioExtra)
        {
            return await _servicioExtraRepository.UpdateAsync(servicioExtra);
        }

        public async Task<bool> DeleteServicioExtraAsync(int id)
        {
            return await _servicioExtraRepository.DeleteAsync(id);
        }
    }
}
