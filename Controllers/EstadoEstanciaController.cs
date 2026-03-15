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

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoEstancia>> GetById(int id)
        {
            var estado = await _estadoEstanciaService.GetEstadoEstanciaByIdAsync(id);
            if (estado == null)
                return NotFound();

            return Ok(estado);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoEstancia>>> GetAll()
        {
            var estados = await _estadoEstanciaService.GetAllEstadosEstanciaAsync();
            return Ok(estados);
        }

        [HttpPost]
        public async Task<ActionResult<EstadoEstancia>> Create([FromBody] CreateEstadoEstanciaRequest request)
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

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
