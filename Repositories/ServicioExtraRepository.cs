using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class ServicioExtraRepository : IServicioExtraRepository
    {
        private readonly AppDbContext _context;

        public ServicioExtraRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServicioExtra?> GetByIdAsync(int id)
        {
            return await _context.ServiciosExtra.FindAsync(id);
        }

        public async Task<IEnumerable<ServicioExtra>> GetAllAsync()
        {
            return await _context.ServiciosExtra.ToListAsync();
        }

        public async Task<ServicioExtra> AddAsync(ServicioExtra servicioExtra)
        {
            _context.ServiciosExtra.Add(servicioExtra);
            await _context.SaveChangesAsync();
            return servicioExtra;
        }

        public async Task<ServicioExtra> UpdateAsync(ServicioExtra servicioExtra)
        {
            _context.ServiciosExtra.Update(servicioExtra);
            await _context.SaveChangesAsync();
            return servicioExtra;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var servicioExtra = await _context.ServiciosExtra.FindAsync(id);
            if (servicioExtra == null)
                return false;

            _context.ServiciosExtra.Remove(servicioExtra);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
