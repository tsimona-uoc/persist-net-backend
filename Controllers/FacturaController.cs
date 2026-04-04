using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Factura;
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

        private FacturaResponse MapToResponse(Factura factura)
        {
            return new FacturaResponse
            {
                Id = factura.Id,
                EstanciaId = factura.EstanciaId,
                ClienteId = factura.ClienteId,
                Descuento = factura.Descuento,
                Total = factura.Total,
                FechaEmision = factura.FechaEmision,
                Pagada = factura.Pagada
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaResponse>> GetFactura(int id)
        {
            var factura = await _facturaService.GetFacturaByIdAsync(id);
            if (factura == null)
                return NotFound();

            return Ok(MapToResponse(factura));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaResponse>>> GetAllFacturas()
        {
            var facturas = await _facturaService.GetAllFacturasAsync();
            return Ok(facturas.Select(MapToResponse));
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<FacturaResponse>>> GetFacturasByCliente(int clienteId)
        {
            var facturas = await _facturaService.GetFacturasByClienteAsync(clienteId);
            return Ok(facturas.Select(MapToResponse));
        }

        [HttpGet("estancia/{estanciaId}")]
        public async Task<ActionResult<IEnumerable<FacturaResponse>>> GetFacturasByEstancia(int estanciaId)
        {
            var facturas = await _facturaService.GetFacturasByEstanciaAsync(estanciaId);
            return Ok(facturas.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<FacturaResponse>> CreateFactura([FromBody] CreateFacturaRequest factura)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdFactura = await _facturaService.CreateFacturaAsync(new Factura
                {
                    EstanciaId = factura.EstanciaId,
                    ClienteId = factura.ClienteId,
                    Descuento = factura.Descuento ?? 0.0m,
                    Total = factura.Total,
                    FechaEmision = factura.FechaEmision ?? DateTime.Now,
                    Pagada = factura.Pagada,
                    LastModifiedBy = "system",
                    LastModifiedAt = DateTime.Now
                });

                return CreatedAtAction(nameof(GetFactura), new { id = createdFactura.Id }, MapToResponse(createdFactura));
            }
            catch (EntityReferenceValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFactura(int id, [FromBody] UpdateFacturaRequest factura)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingFactura = await _facturaService.GetFacturaByIdAsync(id);
            if (existingFactura == null)
                return NotFound();

            existingFactura.EstanciaId = factura.EstanciaId ?? existingFactura.EstanciaId;
            existingFactura.ClienteId = factura.ClienteId ?? existingFactura.ClienteId;
            existingFactura.Descuento = factura.Descuento ?? existingFactura.Descuento;
            existingFactura.Total = factura.Total ?? existingFactura.Total;
            existingFactura.FechaEmision = factura.FechaEmision ?? existingFactura.FechaEmision;
            existingFactura.Pagada = factura.Pagada ?? existingFactura.Pagada;
            existingFactura.LastModifiedBy = "system";
            existingFactura.LastModifiedAt = DateTime.Now;

            await _facturaService.UpdateFacturaAsync(existingFactura);
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
