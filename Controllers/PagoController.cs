using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetPago(int id)
        {
            var pago = await _pagoService.GetPagoByIdAsync(id);
            if (pago == null)
                return NotFound();

            return Ok(pago);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetAllPagos()
        {
            var pagos = await _pagoService.GetAllPagosAsync();
            return Ok(pagos);
        }

        [HttpGet("factura/{facturaId}")]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagosByFactura(int facturaId)
        {
            var pagos = await _pagoService.GetPagosByFacturaAsync(facturaId);
            return Ok(pagos);
        }

        [HttpPost]
        public async Task<ActionResult<Pago>> CreatePago([FromBody] Pago pago)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPago = await _pagoService.CreatePagoAsync(pago);
            return CreatedAtAction(nameof(GetPago), new { id = createdPago.Id }, createdPago);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] Pago pago)
        {
            if (id != pago.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPago = await _pagoService.GetPagoByIdAsync(id);
            if (existingPago == null)
                return NotFound();

            await _pagoService.UpdatePagoAsync(pago);
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
