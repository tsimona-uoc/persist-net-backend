using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class EstadoEstanciaRepository : IEstadoEstanciaRepository
    {
        private readonly AppDbContext _context;

        public EstadoEstanciaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EstadoEstancia?> GetByIdAsync(int id)
        {
            return await _context.Set<EstadoEstancia>().FindAsync(id);
        }

        public async Task<IEnumerable<EstadoEstancia>> GetAllAsync()
        {
            return await _context.Set<EstadoEstancia>().ToListAsync();
        }

        public async Task<EstadoEstancia> AddAsync(EstadoEstancia estadoEstancia)
        {
            _context.Set<EstadoEstancia>().Add(estadoEstancia);
            await _context.SaveChangesAsync();
            return estadoEstancia;
        }

        public async Task<EstadoEstancia> UpdateAsync(EstadoEstancia estadoEstancia)
        {
            _context.Set<EstadoEstancia>().Update(estadoEstancia);
            await _context.SaveChangesAsync();
            return estadoEstancia;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<EstadoEstancia>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<EstadoEstancia>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
