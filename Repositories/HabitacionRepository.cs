using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class HabitacionRepository : IHabitacionRepository
    {
        private readonly AppDbContext _context;

        public HabitacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Habitacion?> GetByIdAsync(int id)
        {
            return await _context.Habitaciones.FindAsync(id);
        }

        public async Task<IEnumerable<Habitacion>> GetAllAsync()
        {
            return await _context.Habitaciones.ToListAsync();
        }

        public async Task<IEnumerable<Habitacion>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.Habitaciones
                .Where(h => h.HotelId == hotelId)
                .ToListAsync();
        }

        public async Task<Habitacion> AddAsync(Habitacion habitacion)
        {
            _context.Habitaciones.Add(habitacion);
            await _context.SaveChangesAsync();
            return habitacion;
        }

        public async Task<Habitacion> UpdateAsync(Habitacion habitacion)
        {
            _context.Habitaciones.Update(habitacion);
            await _context.SaveChangesAsync();
            return habitacion;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null)
                return false;

            _context.Habitaciones.Remove(habitacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
