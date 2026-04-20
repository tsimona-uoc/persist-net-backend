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

        private IQueryable<Estancia> QueryWithRelations()
        {
            return _context.Estancias
                .Include(e => e.EstadoEstancia);
        }

        public async Task<Estancia?> GetByIdAsync(int id)
        {
            return await QueryWithRelations()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Estancia?> GetByIdNoTrackingAsync(int id)
        {
            return await QueryWithRelations()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Estancia>> GetAllAsync()
        {
            return await QueryWithRelations().ToListAsync();
        }

        public async Task<IEnumerable<Estancia>> GetByReservaIdAsync(int reservaId)
        {
            return await QueryWithRelations()
                .Where(e => e.ReservaId == reservaId)
                .ToListAsync();
        }

        public async Task<bool> HasActiveEstanciaByHabitacionIdAsync(int habitacionId, int? excludeEstanciaId = null)
        {
            var now = DateTime.Now;

            return await _context.Estancias.AnyAsync(e =>
                e.Reserva != null &&
                e.Reserva.HabitacionId == habitacionId &&
                (!excludeEstanciaId.HasValue || e.Id != excludeEstanciaId.Value) &&
                e.FechaCheckIn <= now &&
                (e.FechaCheckOut == null || e.FechaCheckOut > now));
        }

        public async Task<Estancia> AddAsync(Estancia estancia)
        {
            _context.Estancias.Add(estancia);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(estancia.Id)
                ?? throw new InvalidOperationException("Could not load the created estancia.");
        }

        public async Task<Estancia> UpdateAsync(Estancia estancia)
        {
            _context.Estancias.Update(estancia);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(estancia.Id)
                ?? throw new InvalidOperationException("Could not load the updated estancia.");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var estancia = await _context.Estancias.FindAsync(id);
            if (estancia == null)
                return false;

            var facturas = await _context.Facturas
                .Where(f => f.EstanciaId == id)
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
                .Where(c => c.EstanciaId == id)
                .ToListAsync();
            _context.ConsumosExtra.RemoveRange(consumos);

            _context.Estancias.Remove(estancia);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
