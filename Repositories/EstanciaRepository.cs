using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class EstanciaRepository : IEstanciaRepository
    {
        private readonly AppDbContext _context;

        public EstanciaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Estancia?> GetByIdAsync(int id)
        {
            return await _context.Estancias.FindAsync(id);
        }

        public async Task<IEnumerable<Estancia>> GetAllAsync()
        {
            return await _context.Estancias.ToListAsync();
        }

        public async Task<IEnumerable<Estancia>> GetByReservaIdAsync(int reservaId)
        {
            return await _context.Estancias
                .Where(e => e.ReservaId == reservaId)
                .ToListAsync();
        }

        public async Task<Estancia> AddAsync(Estancia estancia)
        {
            _context.Estancias.Add(estancia);
            await _context.SaveChangesAsync();
            return estancia;
        }

        public async Task<Estancia> UpdateAsync(Estancia estancia)
        {
            _context.Estancias.Update(estancia);
            await _context.SaveChangesAsync();
            return estancia;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var estancia = await _context.Estancias.FindAsync(id);
            if (estancia == null)
                return false;

            _context.Estancias.Remove(estancia);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
