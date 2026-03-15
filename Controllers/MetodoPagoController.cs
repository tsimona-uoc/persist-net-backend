using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MetodoPagoController : ControllerBase
    {
        private readonly IMetodoPagoService _metodoPagoService;

        public MetodoPagoController(IMetodoPagoService metodoPagoService)
        {
            _metodoPagoService = metodoPagoService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MetodoPago>> GetMetodoPago(int id)
        {
            var metodoPago = await _metodoPagoService.GetMetodoPagoByIdAsync(id);
            if (metodoPago == null)
                return NotFound();

            return Ok(metodoPago);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoPago>>> GetAllMetodosPago()
        {
            var metodosPago = await _metodoPagoService.GetAllMetodosPagoAsync();
            return Ok(metodosPago);
        }

        [HttpPost]
        public async Task<ActionResult<MetodoPago>> CreateMetodoPago([FromBody] MetodoPago metodoPago)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdMetodoPago = await _metodoPagoService.CreateMetodoPagoAsync(metodoPago);
            return CreatedAtAction(nameof(GetMetodoPago), new { id = createdMetodoPago.Id }, createdMetodoPago);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMetodoPago(int id, [FromBody] MetodoPago metodoPago)
        {
            if (id != metodoPago.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingMetodoPago = await _metodoPagoService.GetMetodoPagoByIdAsync(id);
            if (existingMetodoPago == null)
                return NotFound();

            await _metodoPagoService.UpdateMetodoPagoAsync(metodoPago);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetodoPago(int id)
        {
            var success = await _metodoPagoService.DeleteMetodoPagoAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
