using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class TarifaRepository : ITarifaRepository
    {
        private readonly AppDbContext _context;

        public TarifaRepository(AppDbContext context)
        {
            _context = context;
        }

        private IQueryable<Tarifa> QueryWithRelations()
        {
            return _context.Tarifas
                .Include(t => t.Temporada);
        }

        public async Task<Tarifa?> GetByIdAsync(int id)
        {
            return await QueryWithRelations()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tarifa>> GetAllAsync()
        {
            return await QueryWithRelations().ToListAsync();
        }

        public async Task<Tarifa> AddAsync(Tarifa tarifa)
        {
            _context.Tarifas.Add(tarifa);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(tarifa.Id)
                ?? throw new InvalidOperationException("Could not load the created tarifa.");
        }

        public async Task<Tarifa> UpdateAsync(Tarifa tarifa)
        {
            _context.Tarifas.Update(tarifa);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(tarifa.Id)
                ?? throw new InvalidOperationException("Could not load the updated tarifa.");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tarifa = await _context.Tarifas.FindAsync(id);
            if (tarifa == null)
                return false;

            _context.Tarifas.Remove(tarifa);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
