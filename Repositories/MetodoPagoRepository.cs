using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly AppDbContext _context;

        public MetodoPagoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MetodoPago?> GetByIdAsync(int id)
        {
            return await _context.Set<MetodoPago>().FindAsync(id);
        }

        public async Task<IEnumerable<MetodoPago>> GetAllAsync()
        {
            return await _context.Set<MetodoPago>().ToListAsync();
        }

        public async Task<MetodoPago> AddAsync(MetodoPago metodoPago)
        {
            _context.Set<MetodoPago>().Add(metodoPago);
            await _context.SaveChangesAsync();
            return metodoPago;
        }

        public async Task<MetodoPago> UpdateAsync(MetodoPago metodoPago)
        {
            _context.Set<MetodoPago>().Update(metodoPago);
            await _context.SaveChangesAsync();
            return metodoPago;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var metodoPago = await _context.Set<MetodoPago>().FindAsync(id);
            if (metodoPago == null)
                return false;

            var pagos = await _context.Pagos
                .Where(p => p.MetodoPagoId == id)
                .ToListAsync();
            _context.Pagos.RemoveRange(pagos);

            _context.Set<MetodoPago>().Remove(metodoPago);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
