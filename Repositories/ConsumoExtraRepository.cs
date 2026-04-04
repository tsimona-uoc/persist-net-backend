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

        private IQueryable<ConsumoExtra> QueryWithRelations()
        {
            return _context.ConsumosExtra
                .Include(c => c.ServicioExtra);
        }

        public async Task<ConsumoExtra?> GetByIdAsync(int id)
        {
            return await QueryWithRelations()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ConsumoExtra>> GetAllAsync()
        {
            return await QueryWithRelations().ToListAsync();
        }

        public async Task<IEnumerable<ConsumoExtra>> GetByEstanciaIdAsync(int estanciaId)
        {
            return await QueryWithRelations()
                .Where(c => c.EstanciaId == estanciaId)
                .ToListAsync();
        }

        public async Task<ConsumoExtra> AddAsync(ConsumoExtra consumoExtra)
        {
            _context.ConsumosExtra.Add(consumoExtra);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(consumoExtra.Id)
                ?? throw new InvalidOperationException("Could not load the created consumo extra.");
        }

        public async Task<ConsumoExtra> UpdateAsync(ConsumoExtra consumoExtra)
        {
            _context.ConsumosExtra.Update(consumoExtra);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(consumoExtra.Id)
                ?? throw new InvalidOperationException("Could not load the updated consumo extra.");
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
