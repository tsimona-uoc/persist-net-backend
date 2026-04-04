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

        private IQueryable<Reserva> QueryWithRelations()
        {
            return _context.Reservas
                .Include(r => r.Regimen)
                .Include(r => r.EstadoReserva);
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
            return await QueryWithRelations()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await QueryWithRelations().ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByClienteIdAsync(int clienteId)
        {
            return await QueryWithRelations()
                .Where(r => r.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByHabitacionIdAsync(int habitacionId)
        {
            return await QueryWithRelations()
                .Where(r => r.HabitacionId == habitacionId)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingReservationAsync(int habitacionId, DateOnly fechaEntrada, DateOnly fechaSalida, int? excludeReservaId = null)
        {
            return await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == habitacionId &&
                (!excludeReservaId.HasValue || r.Id != excludeReservaId.Value) &&
                fechaEntrada < r.FechaSalida &&
                fechaSalida > r.FechaEntrada);
        }

        public async Task<Reserva> AddAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(reserva.Id)
                ?? throw new InvalidOperationException("Could not load the created reserva.");
        }

        public async Task<Reserva> UpdateAsync(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(reserva.Id)
                ?? throw new InvalidOperationException("Could not load the updated reserva.");
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
