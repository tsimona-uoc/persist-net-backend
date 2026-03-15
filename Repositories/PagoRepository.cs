using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly AppDbContext _context;

        public PagoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pago?> GetByIdAsync(int id)
        {
            return await _context.Pagos.FindAsync(id);
        }

        public async Task<IEnumerable<Pago>> GetAllAsync()
        {
            return await _context.Pagos.ToListAsync();
        }

        public async Task<IEnumerable<Pago>> GetByFacturaIdAsync(int facturaId)
        {
            return await _context.Pagos
                .Where(p => p.FacturaId == facturaId)
                .ToListAsync();
        }

        public async Task<Pago> AddAsync(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
            return pago;
        }

        public async Task<Pago> UpdateAsync(Pago pago)
        {
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
            return pago;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
                return false;

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
