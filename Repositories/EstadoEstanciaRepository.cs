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

            var estancias = await _context.Estancias
                .Where(e => e.EstadoEstanciaId == id)
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

            _context.Set<EstadoEstancia>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
