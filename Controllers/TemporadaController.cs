using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Temporada;
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
        public async Task<ActionResult<Temporada>> CreateTemporada([FromBody] CreateTemporadaRequest temporada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTemporada = await _temporadaService.CreateTemporadaAsync(new Temporada
            {
                Nombre = temporada.Nombre,
                FechaInicio = temporada.FechaInicio,
                FechaFin = temporada.FechaFin,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
            return CreatedAtAction(nameof(GetTemporada), new { id = createdTemporada.Id }, createdTemporada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemporada(int id, [FromBody] UpdateTemporadaRequest temporada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTemporada = await _temporadaService.GetTemporadaByIdAsync(id);
            if (existingTemporada == null)
                return NotFound();

            existingTemporada.Nombre = temporada.Nombre ?? existingTemporada.Nombre;
            existingTemporada.FechaInicio = temporada.FechaInicio ?? existingTemporada.FechaInicio;
            existingTemporada.FechaFin = temporada.FechaFin ?? existingTemporada.FechaFin;
            existingTemporada.LastModifiedBy = "system";
            existingTemporada.LastModifiedAt = DateTime.Now;

            await _temporadaService.UpdateTemporadaAsync(existingTemporada);
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
