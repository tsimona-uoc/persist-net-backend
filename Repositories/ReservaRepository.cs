using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly AppDbContext _context;

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
            return await _context.Reservas.FindAsync(id);
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas.ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Reservas
                .Where(r => r.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByHabitacionIdAsync(int habitacionId)
        {
            return await _context.Reservas
                .Where(r => r.HabitacionId == habitacionId)
                .ToListAsync();
        }

        public async Task<Reserva> AddAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task<Reserva> UpdateAsync(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return false;

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
