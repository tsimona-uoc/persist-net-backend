using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ConsumoExtra>> CreateConsumoExtra([FromBody] ConsumoExtra consumoExtra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdConsumoExtra = await _consumoExtraService.CreateConsumoExtraAsync(consumoExtra);
            return CreatedAtAction(nameof(GetConsumoExtra), new { id = createdConsumoExtra.Id }, createdConsumoExtra);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsumoExtra(int id, [FromBody] ConsumoExtra consumoExtra)
        {
            if (id != consumoExtra.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingConsumoExtra = await _consumoExtraService.GetConsumoExtraByIdAsync(id);
            if (existingConsumoExtra == null)
                return NotFound();

            await _consumoExtraService.UpdateConsumoExtraAsync(consumoExtra);
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
