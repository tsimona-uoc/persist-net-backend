using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.EstadoReserva;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstadoReservaController : ControllerBase
    {
        private readonly IEstadoReservaService _estadoReservaService;

        public EstadoReservaController(IEstadoReservaService estadoReservaService)
        {
            _estadoReservaService = estadoReservaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoReserva>> GetById(int id)
        {
            var estado = await _estadoReservaService.GetEstadoReservaByIdAsync(id);
            if (estado == null)
                return NotFound();

            return Ok(estado);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoReserva>>> GetAll()
        {
            var estados = await _estadoReservaService.GetAllEstadosReservaAsync();
            return Ok(estados);
        }

        [HttpPost]
        public async Task<ActionResult<EstadoReserva>> Create([FromBody] CreateEstadoReservaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _estadoReservaService.CreateEstadoReservaAsync(new EstadoReserva
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion ?? string.Empty,
                Activo = request.Activo,
                LastModifiedAt = DateTime.Now,
                LastModifiedBy = "system"
            });

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEstadoReservaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _estadoReservaService.GetEstadoReservaByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Nombre = request.Nombre ?? existing.Nombre;
            existing.Descripcion = request.Descripcion ?? existing.Descripcion;
            existing.Activo = request.Activo ?? existing.Activo;
            existing.LastModifiedAt = DateTime.Now;
            existing.LastModifiedBy = "system";

            await _estadoReservaService.UpdateEstadoReservaAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _estadoReservaService.DeleteEstadoReservaAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
