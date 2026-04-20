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

            var estancias = await _context.Estancias
                .Where(e => e.ReservaId == id)
                .ToListAsync();

            if (estancias.Count > 0)
            {
                var estanciaIds = estancias.Select(e => e.Id).ToList();

                var facturas = await _context.Facturas
                    .Where(f => estanciaIds.Contains(f.EstanciaId))
                    .ToListAsync();

                if (facturas.Count > 0)
                {
                    var facturaIds = facturas.Select(f => f.Id).ToList();

                    var pagos = await _context.Pagos
                        .Where(p => facturaIds.Contains(p.FacturaId))
                        .ToListAsync();
                    _context.Pagos.RemoveRange(pagos);

                    var lineas = await _context.FacturaLineas
                        .Where(l => facturaIds.Contains(l.FacturaId))
                        .ToListAsync();
                    _context.FacturaLineas.RemoveRange(lineas);

                    _context.Facturas.RemoveRange(facturas);
                }

                var consumos = await _context.ConsumosExtra
                    .Where(c => estanciaIds.Contains(c.EstanciaId))
                    .ToListAsync();
                _context.ConsumosExtra.RemoveRange(consumos);

                _context.Estancias.RemoveRange(estancias);
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
