using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturaController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _facturaService.GetFacturaByIdAsync(id);
            if (factura == null)
                return NotFound();

            return Ok(factura);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetAllFacturas()
        {
            var facturas = await _facturaService.GetAllFacturasAsync();
            return Ok(facturas);
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturasByCliente(int clienteId)
        {
            var facturas = await _facturaService.GetFacturasByClienteAsync(clienteId);
            return Ok(facturas);
        }

        [HttpGet("estancia/{estanciaId}")]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturasByEstancia(int estanciaId)
        {
            var facturas = await _facturaService.GetFacturasByEstanciaAsync(estanciaId);
            return Ok(facturas);
        }

        [HttpPost]
        public async Task<ActionResult<Factura>> CreateFactura([FromBody] Factura factura)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdFactura = await _facturaService.CreateFacturaAsync(factura);
            return CreatedAtAction(nameof(GetFactura), new { id = createdFactura.Id }, createdFactura);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFactura(int id, [FromBody] Factura factura)
        {
            if (id != factura.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingFactura = await _facturaService.GetFacturaByIdAsync(id);
            if (existingFactura == null)
                return NotFound();

            await _facturaService.UpdateFacturaAsync(factura);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var success = await _facturaService.DeleteFacturaAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
