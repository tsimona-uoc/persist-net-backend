using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.TipoHabitacion;
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
        public async Task<ActionResult<TipoHabitacion>> CreateTipoHabitacion([FromBody] CreateTipoHabitacionRequest tipoHabitacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTipoHabitacion = await _tipoHabitacionService.CreateTipoHabitacionAsync(new TipoHabitacion
            {
                Nombre = tipoHabitacion.Nombre,
                Descripcion = tipoHabitacion.Descripcion ?? string.Empty,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
            return CreatedAtAction(nameof(GetTipoHabitacion), new { id = createdTipoHabitacion.Id }, createdTipoHabitacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoHabitacion(int id, [FromBody] UpdateTipoHabitacionRequest tipoHabitacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTipoHabitacion = await _tipoHabitacionService.GetTipoHabitacionByIdAsync(id);
            if (existingTipoHabitacion == null)
                return NotFound();

            existingTipoHabitacion.Nombre = tipoHabitacion.Nombre ?? existingTipoHabitacion.Nombre;
            existingTipoHabitacion.Descripcion = tipoHabitacion.Descripcion ?? existingTipoHabitacion.Descripcion;
            existingTipoHabitacion.LastModifiedBy = "system";
            existingTipoHabitacion.LastModifiedAt = DateTime.Now;

            await _tipoHabitacionService.UpdateTipoHabitacionAsync(existingTipoHabitacion);
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
