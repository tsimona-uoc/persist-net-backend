using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly AppDbContext _context;

        public FacturaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Factura?> GetByIdAsync(int id)
        {
            return await _context.Facturas.FindAsync(id);
        }

        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            return await _context.Facturas.ToListAsync();
        }

        public async Task<IEnumerable<Factura>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Facturas
                .Where(f => f.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Factura>> GetByEstanciaIdAsync(int estanciaId)
        {
            return await _context.Facturas
                .Where(f => f.EstanciaId == estanciaId)
                .ToListAsync();
        }

        public async Task<Factura> AddAsync(Factura factura)
        {
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();
            return factura;
        }

        public async Task<Factura> UpdateAsync(Factura factura)
        {
            _context.Facturas.Update(factura);
            await _context.SaveChangesAsync();
            return factura;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
                return false;

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
