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

        private TemporadaResponse MapToResponse(Temporada temporada)
        {
            return new TemporadaResponse
            {
                Id = temporada.Id,
                Nombre = temporada.Nombre,
                FechaInicio = temporada.FechaInicio,
                FechaFin = temporada.FechaFin
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TemporadaResponse>> GetTemporada(int id)
        {
            var temporada = await _temporadaService.GetTemporadaByIdAsync(id);
            if (temporada == null)
                return NotFound();

            return Ok(MapToResponse(temporada));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemporadaResponse>>> GetAllTemporadas()
        {
            var temporadas = await _temporadaService.GetAllTemporadasAsync();
            return Ok(temporadas.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<TemporadaResponse>> CreateTemporada([FromBody] CreateTemporadaRequest temporada)
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
            return CreatedAtAction(nameof(GetTemporada), new { id = createdTemporada.Id }, MapToResponse(createdTemporada));
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
