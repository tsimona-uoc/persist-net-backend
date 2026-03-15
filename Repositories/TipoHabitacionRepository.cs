using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class TipoHabitacionRepository : ITipoHabitacionRepository
    {
        private readonly AppDbContext _context;

        public TipoHabitacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TipoHabitacion?> GetByIdAsync(int id)
        {
            return await _context.TiposHabitacion.FindAsync(id);
        }

        public async Task<IEnumerable<TipoHabitacion>> GetAllAsync()
        {
            return await _context.TiposHabitacion.ToListAsync();
        }

        public async Task<TipoHabitacion> AddAsync(TipoHabitacion tipoHabitacion)
        {
            _context.TiposHabitacion.Add(tipoHabitacion);
            await _context.SaveChangesAsync();
            return tipoHabitacion;
        }

        public async Task<TipoHabitacion> UpdateAsync(TipoHabitacion tipoHabitacion)
        {
            _context.TiposHabitacion.Update(tipoHabitacion);
            await _context.SaveChangesAsync();
            return tipoHabitacion;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tipoHabitacion = await _context.TiposHabitacion.FindAsync(id);
            if (tipoHabitacion == null)
                return false;

            _context.TiposHabitacion.Remove(tipoHabitacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
