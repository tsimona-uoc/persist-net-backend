using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.EstadoEstancia;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstadoEstanciaController : ControllerBase
    {
        private readonly IEstadoEstanciaService _estadoEstanciaService;

        public EstadoEstanciaController(IEstadoEstanciaService estadoEstanciaService)
        {
            _estadoEstanciaService = estadoEstanciaService;
        }

        private EstadoEstanciaResponse MapToResponse(EstadoEstancia estado)
        {
            return new EstadoEstanciaResponse
            {
                Id = estado.Id,
                Nombre = estado.Nombre,
                Descripcion = estado.Descripcion,
                Activo = estado.Activo
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoEstanciaResponse>> GetById(int id)
        {
            var estado = await _estadoEstanciaService.GetEstadoEstanciaByIdAsync(id);
            if (estado == null)
                return NotFound();

            return Ok(MapToResponse(estado));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoEstanciaResponse>>> GetAll()
        {
            var estados = await _estadoEstanciaService.GetAllEstadosEstanciaAsync();
            return Ok(estados.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<EstadoEstanciaResponse>> Create([FromBody] CreateEstadoEstanciaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _estadoEstanciaService.CreateEstadoEstanciaAsync(new EstadoEstancia
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEstadoEstanciaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _estadoEstanciaService.GetEstadoEstanciaByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Nombre = request.Nombre ?? existing.Nombre;
            existing.Descripcion = request.Descripcion ?? existing.Descripcion;
            existing.Activo = request.Activo ?? existing.Activo;
            existing.LastModifiedAt = DateTime.Now;
            existing.LastModifiedBy = "system";

            await _estadoEstanciaService.UpdateEstadoEstanciaAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _estadoEstanciaService.DeleteEstadoEstanciaAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
