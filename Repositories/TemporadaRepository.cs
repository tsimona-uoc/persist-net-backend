using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class TemporadaRepository : ITemporadaRepository
    {
        private readonly AppDbContext _context;

        public TemporadaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Temporada?> GetByIdAsync(int id)
        {
            return await _context.Temporadas.FindAsync(id);
        }

        public async Task<IEnumerable<Temporada>> GetAllAsync()
        {
            return await _context.Temporadas.ToListAsync();
        }

        public async Task<Temporada> AddAsync(Temporada temporada)
        {
            _context.Temporadas.Add(temporada);
            await _context.SaveChangesAsync();
            return temporada;
        }

        public async Task<Temporada> UpdateAsync(Temporada temporada)
        {
            _context.Temporadas.Update(temporada);
            await _context.SaveChangesAsync();
            return temporada;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var temporada = await _context.Temporadas.FindAsync(id);
            if (temporada == null)
                return false;

            _context.Temporadas.Remove(temporada);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
