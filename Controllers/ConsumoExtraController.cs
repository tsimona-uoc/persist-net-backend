using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.ConsumoExtra;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConsumoExtraController : ControllerBase
    {
        private readonly IConsumoExtraService _consumoExtraService;

        public ConsumoExtraController(IConsumoExtraService consumoExtraService)
        {
            _consumoExtraService = consumoExtraService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsumoExtra>> GetConsumoExtra(int id)
        {
            var consumoExtra = await _consumoExtraService.GetConsumoExtraByIdAsync(id);
            if (consumoExtra == null)
                return NotFound();

            return Ok(consumoExtra);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsumoExtra>>> GetAllConsumosExtra()
        {
            var consumosExtra = await _consumoExtraService.GetAllConsumosExtraAsync();
            return Ok(consumosExtra);
        }

        [HttpGet("estancia/{estanciaId}")]
        public async Task<ActionResult<IEnumerable<ConsumoExtra>>> GetConsumoExtrasByEstancia(int estanciaId)
        {
            var consumosExtra = await _consumoExtraService.GetConsumoExtrasByEstanciaAsync(estanciaId);
            return Ok(consumosExtra);
        }

        [HttpPost]
        public async Task<ActionResult<ConsumoExtra>> CreateConsumoExtra([FromBody] CreateConsumoExtraRequest consumoExtra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdConsumoExtra = await _consumoExtraService.CreateConsumoExtraAsync(new ConsumoExtra
            {
                EstanciaId = consumoExtra.EstanciaId,
                ServicioExtraId = consumoExtra.ServicioExtraId,
                Cantidad = consumoExtra.Cantidad,
                PrecioUnitario = consumoExtra.PrecioUnitario,
                Fecha = consumoExtra.Fecha,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });

            return CreatedAtAction(nameof(GetConsumoExtra), new { id = createdConsumoExtra.Id }, createdConsumoExtra);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsumoExtra(int id, [FromBody] UpdateConsumoExtraRequest consumoExtra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingConsumoExtra = await _consumoExtraService.GetConsumoExtraByIdAsync(id);
            if (existingConsumoExtra == null)
                return NotFound();

            existingConsumoExtra.EstanciaId = consumoExtra.EstanciaId ?? existingConsumoExtra.EstanciaId;
            existingConsumoExtra.ServicioExtraId = consumoExtra.ServicioExtraId ?? existingConsumoExtra.ServicioExtraId;
            existingConsumoExtra.Cantidad = consumoExtra.Cantidad ?? existingConsumoExtra.Cantidad;
            existingConsumoExtra.PrecioUnitario = consumoExtra.PrecioUnitario ?? existingConsumoExtra.PrecioUnitario;
            existingConsumoExtra.Fecha = consumoExtra.Fecha ?? existingConsumoExtra.Fecha;
            existingConsumoExtra.LastModifiedBy = "system";
            existingConsumoExtra.LastModifiedAt = DateTime.Now;

            await _consumoExtraService.UpdateConsumoExtraAsync(existingConsumoExtra);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsumoExtra(int id)
        {
            var success = await _consumoExtraService.DeleteConsumoExtraAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
