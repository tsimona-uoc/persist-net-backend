using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Regimen>> CreateRegimen([FromBody] Regimen regimen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRegimen = await _regimenService.CreateRegimenAsync(regimen);
            return CreatedAtAction(nameof(GetRegimen), new { id = createdRegimen.Id }, createdRegimen);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegimen(int id, [FromBody] Regimen regimen)
        {
            if (id != regimen.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingRegimen = await _regimenService.GetRegimenByIdAsync(id);
            if (existingRegimen == null)
                return NotFound();

            await _regimenService.UpdateRegimenAsync(regimen);
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
