using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Tarifa;
using persist_net_backend.DTOs.Temporada;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TarifaController : ControllerBase
    {
        private readonly ITarifaService _tarifaService;

        public TarifaController(ITarifaService tarifaService)
        {
            _tarifaService = tarifaService;
        }

        private TemporadaResponse MapTemporadaToResponse(Temporada temporada)
        {
            return new TemporadaResponse
            {
                Id = temporada.Id,
                Nombre = temporada.Nombre,
                FechaInicio = temporada.FechaInicio,
                FechaFin = temporada.FechaFin
            };
        }

        private TarifaResponse MapToResponse(Tarifa tarifa)
        {
            return new TarifaResponse
            {
                Id = tarifa.Id,
                PrecioNoche = tarifa.PrecioNoche,
                Temporada = tarifa.Temporada != null ? MapTemporadaToResponse(tarifa.Temporada) : throw new InvalidOperationException("Temporada is required")
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TarifaResponse>> GetTarifa(int id)
        {
            var tarifa = await _tarifaService.GetTarifaByIdAsync(id);
            if (tarifa == null)
                return NotFound();

            return Ok(MapToResponse(tarifa));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarifaResponse>>> GetAllTarifas()
        {
            var tarifas = await _tarifaService.GetAllTarifasAsync();
            return Ok(tarifas.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<TarifaResponse>> CreateTarifa([FromBody] CreateTarifaRequest tarifa)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdTarifa = await _tarifaService.CreateTarifaAsync(new Tarifa
                {
                    TemporadaId = tarifa.TemporadaId,
                    PrecioNoche = tarifa.PrecioNoche,
                    LastModifiedBy = "system",
                    LastModifiedAt = DateTime.Now
                });

                return CreatedAtAction(nameof(GetTarifa), new { id = createdTarifa.Id }, MapToResponse(createdTarifa));
            }
            catch (EntityReferenceValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTarifa(int id, [FromBody] UpdateTarifaRequest tarifa)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTarifa = await _tarifaService.GetTarifaByIdAsync(id);
            if (existingTarifa == null)
                return NotFound();

            existingTarifa.TemporadaId = tarifa.TemporadaId ?? existingTarifa.TemporadaId;
            existingTarifa.PrecioNoche = tarifa.PrecioNoche ?? existingTarifa.PrecioNoche;
            existingTarifa.LastModifiedBy = "system";
            existingTarifa.LastModifiedAt = DateTime.Now;

            await _tarifaService.UpdateTarifaAsync(existingTarifa);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarifa(int id)
        {
            var success = await _tarifaService.DeleteTarifaAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
