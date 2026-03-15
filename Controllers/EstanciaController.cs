using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstanciaController : ControllerBase
    {
        private readonly IEstanciaService _estanciaService;

        public EstanciaController(IEstanciaService estanciaService)
        {
            _estanciaService = estanciaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estancia>> GetEstancia(int id)
        {
            var estancia = await _estanciaService.GetEstanciaByIdAsync(id);
            if (estancia == null)
                return NotFound();

            return Ok(estancia);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estancia>>> GetAllEstancias()
        {
            var estancias = await _estanciaService.GetAllEstanciasAsync();
            return Ok(estancias);
        }

        [HttpGet("reserva/{reservaId}")]
        public async Task<ActionResult<IEnumerable<Estancia>>> GetEstanciasByReserva(int reservaId)
        {
            var estancias = await _estanciaService.GetEstanciasByReservaAsync(reservaId);
            return Ok(estancias);
        }

        [HttpPost]
        public async Task<ActionResult<Estancia>> CreateEstancia([FromBody] Estancia estancia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEstancia = await _estanciaService.CreateEstanciaAsync(estancia);
            return CreatedAtAction(nameof(GetEstancia), new { id = createdEstancia.Id }, createdEstancia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstancia(int id, [FromBody] Estancia estancia)
        {
            if (id != estancia.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEstancia = await _estanciaService.GetEstanciaByIdAsync(id);
            if (existingEstancia == null)
                return NotFound();

            await _estanciaService.UpdateEstanciaAsync(estancia);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstancia(int id)
        {
            var success = await _estanciaService.DeleteEstanciaAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
