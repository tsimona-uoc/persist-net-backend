using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Regimen;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RegimenController : ControllerBase
    {
        private readonly IRegimenService _regimenService;

        public RegimenController(IRegimenService regimenService)
        {
            _regimenService = regimenService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Regimen>> GetRegimen(int id)
        {
            var regimen = await _regimenService.GetRegimenByIdAsync(id);
            if (regimen == null)
                return NotFound();

            return Ok(regimen);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regimen>>> GetAllRegimenes()
        {
            var regimenes = await _regimenService.GetAllRegimenesAsync();
            return Ok(regimenes);
        }

        [HttpPost]
        public async Task<ActionResult<Regimen>> CreateRegimen([FromBody] CreateRegimenRequest regimen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRegimen = await _regimenService.CreateRegimenAsync(new Regimen
            {
                Nombre = regimen.Nombre,
                Descripcion = regimen.Descripcion ?? string.Empty,
                Activo = regimen.Activo,
                LastModifiedAt = DateTime.Now,
                LastModifiedBy = "system"
            });
            return CreatedAtAction(nameof(GetRegimen), new { id = createdRegimen.Id }, createdRegimen);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegimen(int id, [FromBody] UpdateRegimenRequest regimen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingRegimen = await _regimenService.GetRegimenByIdAsync(id);
            if (existingRegimen == null)
                return NotFound();

            existingRegimen.Nombre = regimen.Nombre ?? existingRegimen.Nombre;
            existingRegimen.Descripcion = regimen.Descripcion ?? existingRegimen.Descripcion;
            existingRegimen.Activo = regimen.Activo ?? existingRegimen.Activo;
            existingRegimen.LastModifiedAt = DateTime.Now;
            existingRegimen.LastModifiedBy = "system";

            await _regimenService.UpdateRegimenAsync(existingRegimen);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegimen(int id)
        {
            var success = await _regimenService.DeleteRegimenAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
