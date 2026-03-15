using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TemporadaController : ControllerBase
    {
        private readonly ITemporadaService _temporadaService;

        public TemporadaController(ITemporadaService temporadaService)
        {
            _temporadaService = temporadaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Temporada>> GetTemporada(int id)
        {
            var temporada = await _temporadaService.GetTemporadaByIdAsync(id);
            if (temporada == null)
                return NotFound();

            return Ok(temporada);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Temporada>>> GetAllTemporadas()
        {
            var temporadas = await _temporadaService.GetAllTemporadasAsync();
            return Ok(temporadas);
        }

        [HttpPost]
        public async Task<ActionResult<Temporada>> CreateTemporada([FromBody] Temporada temporada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTemporada = await _temporadaService.CreateTemporadaAsync(temporada);
            return CreatedAtAction(nameof(GetTemporada), new { id = createdTemporada.Id }, createdTemporada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemporada(int id, [FromBody] Temporada temporada)
        {
            if (id != temporada.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTemporada = await _temporadaService.GetTemporadaByIdAsync(id);
            if (existingTemporada == null)
                return NotFound();

            await _temporadaService.UpdateTemporadaAsync(temporada);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemporada(int id)
        {
            var success = await _temporadaService.DeleteTemporadaAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
