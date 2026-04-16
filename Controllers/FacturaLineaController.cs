using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacturaLineaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacturaLineaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaLinea>>> GetFacturaLineas()
        {
            return await _context.FacturaLineas.ToListAsync();
        }

        [HttpGet("factura/{facturaId}")]
        public async Task<ActionResult<IEnumerable<FacturaLinea>>> GetLineasPorFactura(int facturaId)
        {
            return await _context.FacturaLineas
                                 .Where(l => l.FacturaId == facturaId)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaLinea>> GetFacturaLinea(int id)
        {
            var facturaLinea = await _context.FacturaLineas.FindAsync(id);

            if (facturaLinea == null)
            {
                return NotFound();
            }

            return facturaLinea;
        }

        [HttpPost]
        public async Task<ActionResult<FacturaLinea>> PostFacturaLinea(FacturaLinea facturaLinea)
        {
            if (facturaLinea.Fecha == default)
            {
                facturaLinea.Fecha = DateTime.Now;
            }

            _context.FacturaLineas.Add(facturaLinea);
            await _context.SaveChangesAsync();

            // Actualizamos el total de la factura principal
            await UpdateFacturaTotal(facturaLinea.FacturaId);

            return CreatedAtAction(nameof(GetFacturaLinea), new { id = facturaLinea.Id }, facturaLinea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacturaLinea(int id, FacturaLinea facturaLinea)
        {
            if (id != facturaLinea.Id)
            {
                return BadRequest();
            }

            _context.Entry(facturaLinea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await UpdateFacturaTotal(facturaLinea.FacturaId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaLineaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacturaLinea(int id)
        {
            var facturaLinea = await _context.FacturaLineas.FindAsync(id);
            if (facturaLinea == null)
            {
                return NotFound();
            }

            var facturaId = facturaLinea.FacturaId;

            _context.FacturaLineas.Remove(facturaLinea);
            await _context.SaveChangesAsync();

            await UpdateFacturaTotal(facturaId);

            return NoContent();
        }

        private bool FacturaLineaExists(int id)
        {
            return _context.FacturaLineas.Any(e => e.Id == id);
        }

        private async Task UpdateFacturaTotal(int facturaId)
        {
            var factura = await _context.Facturas.FindAsync(facturaId);
            if (factura != null)
            {
                factura.Total = await _context.FacturaLineas
                    .Where(l => l.FacturaId == facturaId)
                    .SumAsync(l => l.Monto);
                
                _context.Entry(factura).State = EntityState.Modified;
                // Guardamos los cambios en la factura
                await _context.SaveChangesAsync();
            }
        }
    }
}
