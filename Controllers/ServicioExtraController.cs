using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioExtra>> GetServicioExtra(int id)
        {
            var servicioExtra = await _servicioExtraService.GetServicioExtraByIdAsync(id);
            if (servicioExtra == null)
                return NotFound();

            return Ok(servicioExtra);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioExtra>>> GetAllServiciosExtra()
        {
            var serviciosExtra = await _servicioExtraService.GetAllServiciosExtraAsync();
            return Ok(serviciosExtra);
        }

        [HttpPost]
        public async Task<ActionResult<ServicioExtra>> CreateServicioExtra([FromBody] ServicioExtra servicioExtra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdServicioExtra = await _servicioExtraService.CreateServicioExtraAsync(servicioExtra);
            return CreatedAtAction(nameof(GetServicioExtra), new { id = createdServicioExtra.Id }, createdServicioExtra);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicioExtra(int id, [FromBody] ServicioExtra servicioExtra)
        {
            if (id != servicioExtra.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingServicioExtra = await _servicioExtraService.GetServicioExtraByIdAsync(id);
            if (existingServicioExtra == null)
                return NotFound();

            await _servicioExtraService.UpdateServicioExtraAsync(servicioExtra);
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
