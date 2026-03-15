using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class EstadoHabitacionRepository : IEstadoHabitacionRepository
    {
        private readonly AppDbContext _context;

        public EstadoHabitacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EstadoHabitacion?> GetByIdAsync(int id)
        {
            return await _context.Set<EstadoHabitacion>().FindAsync(id);
        }

        public async Task<IEnumerable<EstadoHabitacion>> GetAllAsync()
        {
            return await _context.Set<EstadoHabitacion>().ToListAsync();
        }

        public async Task<EstadoHabitacion> AddAsync(EstadoHabitacion estadoHabitacion)
        {
            _context.Set<EstadoHabitacion>().Add(estadoHabitacion);
            await _context.SaveChangesAsync();
            return estadoHabitacion;
        }

        public async Task<EstadoHabitacion> UpdateAsync(EstadoHabitacion estadoHabitacion)
        {
            _context.Set<EstadoHabitacion>().Update(estadoHabitacion);
            await _context.SaveChangesAsync();
            return estadoHabitacion;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<EstadoHabitacion>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<EstadoHabitacion>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
