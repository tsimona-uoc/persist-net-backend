using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.EstadoHabitacion;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstadoHabitacionController : ControllerBase
    {
        private readonly IEstadoHabitacionService _estadoHabitacionService;

        public EstadoHabitacionController(IEstadoHabitacionService estadoHabitacionService)
        {
            _estadoHabitacionService = estadoHabitacionService;
        }

        private EstadoHabitacionResponse MapToResponse(EstadoHabitacion estado)
        {
            return new EstadoHabitacionResponse
            {
                Id = estado.Id,
                Nombre = estado.Nombre,
                Descripcion = estado.Descripcion,
                Activo = estado.Activo
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoHabitacionResponse>> GetById(int id)
        {
            var estado = await _estadoHabitacionService.GetEstadoHabitacionByIdAsync(id);
            if (estado == null)
                return NotFound();

            return Ok(MapToResponse(estado));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoHabitacionResponse>>> GetAll()
        {
            var estados = await _estadoHabitacionService.GetAllEstadosHabitacionAsync();
            return Ok(estados.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<EstadoHabitacionResponse>> Create([FromBody] CreateEstadoHabitacionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _estadoHabitacionService.CreateEstadoHabitacionAsync(new EstadoHabitacion
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion ?? string.Empty,
                Activo = request.Activo,
                LastModifiedAt = DateTime.Now,
                LastModifiedBy = "system"
            });

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToResponse(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEstadoHabitacionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _estadoHabitacionService.GetEstadoHabitacionByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Nombre = request.Nombre ?? existing.Nombre;
            existing.Descripcion = request.Descripcion ?? existing.Descripcion;
            existing.Activo = request.Activo ?? existing.Activo;
            existing.LastModifiedAt = DateTime.Now;
            existing.LastModifiedBy = "system";

            await _estadoHabitacionService.UpdateEstadoHabitacionAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _estadoHabitacionService.DeleteEstadoHabitacionAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
