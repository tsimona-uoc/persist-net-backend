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

            var reservas = await _context.Reservas
                .Where(r => r.EstadoReservaId == id)
                .ToListAsync();

            if (reservas.Count > 0)
            {
                var reservaIds = reservas.Select(r => r.Id).ToList();

                var estancias = await _context.Estancias
                    .Where(e => reservaIds.Contains(e.ReservaId))
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

                _context.Reservas.RemoveRange(reservas);
            }

            _context.Set<EstadoReserva>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
