using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Tarifa;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Tarifa>> GetTarifa(int id)
        {
            var tarifa = await _tarifaService.GetTarifaByIdAsync(id);
            if (tarifa == null)
                return NotFound();

            return Ok(tarifa);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarifa>>> GetAllTarifas()
        {
            var tarifas = await _tarifaService.GetAllTarifasAsync();
            return Ok(tarifas);
        }

        [HttpPost]
        public async Task<ActionResult<Tarifa>> CreateTarifa([FromBody] CreateTarifaRequest tarifa)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTarifa = await _tarifaService.CreateTarifaAsync(new Tarifa
            {
                TemporadaId = tarifa.TemporadaId,
                PrecioNoche = tarifa.PrecioNoche,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
            return CreatedAtAction(nameof(GetTarifa), new { id = createdTarifa.Id }, createdTarifa);
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
