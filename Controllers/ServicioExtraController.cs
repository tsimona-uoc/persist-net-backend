using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.ServicioExtra;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServicioExtraController : ControllerBase
    {
        private readonly IServicioExtraService _servicioExtraService;

        public ServicioExtraController(IServicioExtraService servicioExtraService)
        {
            _servicioExtraService = servicioExtraService;
        }

        private ServicioExtraResponse MapToResponse(ServicioExtra servicioExtra)
        {
            return new ServicioExtraResponse
            {
                Id = servicioExtra.Id,
                Nombre = servicioExtra.Nombre,
                Descripcion = servicioExtra.Descripcion,
                PrecioBase = servicioExtra.PrecioBase
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioExtraResponse>> GetServicioExtra(int id)
        {
            var servicioExtra = await _servicioExtraService.GetServicioExtraByIdAsync(id);
            if (servicioExtra == null)
                return NotFound();

            return Ok(MapToResponse(servicioExtra));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioExtraResponse>>> GetAllServiciosExtra()
        {
            var serviciosExtra = await _servicioExtraService.GetAllServiciosExtraAsync();
            return Ok(serviciosExtra.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<ServicioExtraResponse>> CreateServicioExtra([FromBody] CreateServicioExtraRequest servicioExtra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdServicioExtra = await _servicioExtraService.CreateServicioExtraAsync(new ServicioExtra
            {
                Nombre = servicioExtra.Nombre,
                Descripcion = servicioExtra.Descripcion ?? string.Empty,
                PrecioBase = servicioExtra.PrecioBase,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
            return CreatedAtAction(nameof(GetServicioExtra), new { id = createdServicioExtra.Id }, MapToResponse(createdServicioExtra));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicioExtra(int id, [FromBody] UpdateServicioExtraRequest servicioExtra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingServicioExtra = await _servicioExtraService.GetServicioExtraByIdAsync(id);
            if (existingServicioExtra == null)
                return NotFound();

            existingServicioExtra.Nombre = servicioExtra.Nombre ?? existingServicioExtra.Nombre;
            existingServicioExtra.Descripcion = servicioExtra.Descripcion ?? existingServicioExtra.Descripcion;
            existingServicioExtra.PrecioBase = servicioExtra.PrecioBase ?? existingServicioExtra.PrecioBase;
            existingServicioExtra.LastModifiedBy = "system";
            existingServicioExtra.LastModifiedAt = DateTime.Now;

            await _servicioExtraService.UpdateServicioExtraAsync(existingServicioExtra);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicioExtra(int id)
        {
            var success = await _servicioExtraService.DeleteServicioExtraAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
