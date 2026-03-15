using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TipoHabitacionController : ControllerBase
    {
        private readonly ITipoHabitacionService _tipoHabitacionService;

        public TipoHabitacionController(ITipoHabitacionService tipoHabitacionService)
        {
            _tipoHabitacionService = tipoHabitacionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoHabitacion>> GetTipoHabitacion(int id)
        {
            var tipoHabitacion = await _tipoHabitacionService.GetTipoHabitacionByIdAsync(id);
            if (tipoHabitacion == null)
                return NotFound();

            return Ok(tipoHabitacion);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoHabitacion>>> GetAllTiposHabitacion()
        {
            var tiposHabitacion = await _tipoHabitacionService.GetAllTiposHabitacionAsync();
            return Ok(tiposHabitacion);
        }

        [HttpPost]
        public async Task<ActionResult<TipoHabitacion>> CreateTipoHabitacion([FromBody] TipoHabitacion tipoHabitacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTipoHabitacion = await _tipoHabitacionService.CreateTipoHabitacionAsync(tipoHabitacion);
            return CreatedAtAction(nameof(GetTipoHabitacion), new { id = createdTipoHabitacion.Id }, createdTipoHabitacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoHabitacion(int id, [FromBody] TipoHabitacion tipoHabitacion)
        {
            if (id != tipoHabitacion.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTipoHabitacion = await _tipoHabitacionService.GetTipoHabitacionByIdAsync(id);
            if (existingTipoHabitacion == null)
                return NotFound();

            await _tipoHabitacionService.UpdateTipoHabitacionAsync(tipoHabitacion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoHabitacion(int id)
        {
            var success = await _tipoHabitacionService.DeleteTipoHabitacionAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
