using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class ConsumoExtraRepository : IConsumoExtraRepository
    {
        private readonly AppDbContext _context;

        public ConsumoExtraRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConsumoExtra?> GetByIdAsync(int id)
        {
            return await _context.ConsumosExtra.FindAsync(id);
        }

        public async Task<IEnumerable<ConsumoExtra>> GetAllAsync()
        {
            return await _context.ConsumosExtra.ToListAsync();
        }

        public async Task<IEnumerable<ConsumoExtra>> GetByEstanciaIdAsync(int estanciaId)
        {
            return await _context.ConsumosExtra
                .Where(c => c.EstanciaId == estanciaId)
                .ToListAsync();
        }

        public async Task<ConsumoExtra> AddAsync(ConsumoExtra consumoExtra)
        {
            _context.ConsumosExtra.Add(consumoExtra);
            await _context.SaveChangesAsync();
            return consumoExtra;
        }

        public async Task<ConsumoExtra> UpdateAsync(ConsumoExtra consumoExtra)
        {
            _context.ConsumosExtra.Update(consumoExtra);
            await _context.SaveChangesAsync();
            return consumoExtra;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var consumoExtra = await _context.ConsumosExtra.FindAsync(id);
            if (consumoExtra == null)
                return false;

            _context.ConsumosExtra.Remove(consumoExtra);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
