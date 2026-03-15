using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class RegimenRepository : IRegimenRepository
    {
        private readonly AppDbContext _context;

        public RegimenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Regimen?> GetByIdAsync(int id)
        {
            return await _context.Set<Regimen>().FindAsync(id);
        }

        public async Task<IEnumerable<Regimen>> GetAllAsync()
        {
            return await _context.Set<Regimen>().ToListAsync();
        }

        public async Task<Regimen> AddAsync(Regimen regimen)
        {
            _context.Set<Regimen>().Add(regimen);
            await _context.SaveChangesAsync();
            return regimen;
        }

        public async Task<Regimen> UpdateAsync(Regimen regimen)
        {
            _context.Set<Regimen>().Update(regimen);
            await _context.SaveChangesAsync();
            return regimen;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var regimen = await _context.Set<Regimen>().FindAsync(id);
            if (regimen == null)
                return false;

            _context.Set<Regimen>().Remove(regimen);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
