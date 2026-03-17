using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class EstadoReservaRepository : IEstadoReservaRepository
    {
        private readonly AppDbContext _context;

        public EstadoReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EstadoReserva?> GetByIdAsync(int id)
        {
            return await _context.Set<EstadoReserva>().FindAsync(id);
        }

        public async Task<IEnumerable<EstadoReserva>> GetAllAsync()
        {
            return await _context.Set<EstadoReserva>().ToListAsync();
        }

        public async Task<EstadoReserva> AddAsync(EstadoReserva estadoReserva)
        {
            _context.Set<EstadoReserva>().Add(estadoReserva);
            await _context.SaveChangesAsync();
            return estadoReserva;
        }

        public async Task<EstadoReserva> UpdateAsync(EstadoReserva estadoReserva)
        {
            _context.Set<EstadoReserva>().Update(estadoReserva);
            await _context.SaveChangesAsync();
            return estadoReserva;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<EstadoReserva>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<EstadoReserva>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
