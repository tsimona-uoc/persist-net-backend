using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.MetodoPago;
using persist_net_backend.DTOs.Pago;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _pagoService;

        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        private MetodoPagoResponse MapMetodoPagoToResponse(MetodoPago metodoPago)
        {
            return new MetodoPagoResponse
            {
                Id = metodoPago.Id,
                Nombre = metodoPago.Nombre,
                Descripcion = metodoPago.Descripcion
            };
        }

        private PagoResponse MapToResponse(Pago pago)
        {
            return new PagoResponse
            {
                Id = pago.Id,
                FacturaId = pago.FacturaId,
                Importe = pago.Importe,
                FechaPago = pago.FechaPago,
                MetodoPago = pago.MetodoPago != null ? MapMetodoPagoToResponse(pago.MetodoPago) : throw new InvalidOperationException("MetodoPago is required")
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagoResponse>> GetPago(int id)
        {
            var pago = await _pagoService.GetPagoByIdAsync(id);
            if (pago == null)
                return NotFound();

            return Ok(MapToResponse(pago));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagoResponse>>> GetAllPagos()
        {
            var pagos = await _pagoService.GetAllPagosAsync();
            return Ok(pagos.Select(MapToResponse));
        }

        [HttpGet("factura/{facturaId}")]
        public async Task<ActionResult<IEnumerable<PagoResponse>>> GetPagosByFactura(int facturaId)
        {
            var pagos = await _pagoService.GetPagosByFacturaAsync(facturaId);
            return Ok(pagos.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<PagoResponse>> CreatePago([FromBody] CreatePagoRequest pago)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPago = await _pagoService.CreatePagoAsync(new Pago
            {
                FacturaId = pago.FacturaId,
                MetodoPagoId = pago.MetodoPagoId,
                Importe = pago.Importe,
                FechaPago = pago.FechaPago ?? DateTime.Now,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
            return CreatedAtAction(nameof(GetPago), new { id = createdPago.Id }, MapToResponse(createdPago));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] UpdatePagoRequest pago)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPago = await _pagoService.GetPagoByIdAsync(id);
            if (existingPago == null)
                return NotFound();

            existingPago.FacturaId = pago.FacturaId ?? existingPago.FacturaId;
            existingPago.MetodoPagoId = pago.MetodoPagoId ?? existingPago.MetodoPagoId;
            existingPago.Importe = pago.Importe ?? existingPago.Importe;
            existingPago.FechaPago = pago.FechaPago ?? existingPago.FechaPago;
            existingPago.LastModifiedBy = "system";
            existingPago.LastModifiedAt = DateTime.Now;

            await _pagoService.UpdatePagoAsync(existingPago);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var success = await _pagoService.DeletePagoAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
