using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.MetodoPago;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MetodoPagoController : ControllerBase
    {
        private readonly IMetodoPagoService _metodoPagoService;

        public MetodoPagoController(IMetodoPagoService metodoPagoService)
        {
            _metodoPagoService = metodoPagoService;
        }

        private MetodoPagoResponse MapToResponse(MetodoPago metodoPago)
        {
            return new MetodoPagoResponse
            {
                Id = metodoPago.Id,
                Nombre = metodoPago.Nombre,
                Descripcion = metodoPago.Descripcion,
                Activo = metodoPago.Activo
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MetodoPagoResponse>> GetMetodoPago(int id)
        {
            var metodoPago = await _metodoPagoService.GetMetodoPagoByIdAsync(id);
            if (metodoPago == null)
                return NotFound();

            return Ok(MapToResponse(metodoPago));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoPagoResponse>>> GetAllMetodosPago()
        {
            var metodosPago = await _metodoPagoService.GetAllMetodosPagoAsync();
            return Ok(metodosPago.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<MetodoPagoResponse>> CreateMetodoPago([FromBody] CreateMetodoPagoRequest metodoPago)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdMetodoPago = await _metodoPagoService.CreateMetodoPagoAsync(new MetodoPago
            {
                Nombre = metodoPago.Nombre,
                Descripcion = metodoPago.Descripcion ?? string.Empty,
                Activo = metodoPago.Activo
            });
            return CreatedAtAction(nameof(GetMetodoPago), new { id = createdMetodoPago.Id }, MapToResponse(createdMetodoPago));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMetodoPago(int id, [FromBody] UpdateMetodoPagoRequest metodoPago)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingMetodoPago = await _metodoPagoService.GetMetodoPagoByIdAsync(id);
            if (existingMetodoPago == null)
                return NotFound();

            existingMetodoPago.Nombre = metodoPago.Nombre ?? existingMetodoPago.Nombre;
            existingMetodoPago.Descripcion = metodoPago.Descripcion ?? existingMetodoPago.Descripcion;
            existingMetodoPago.Activo = metodoPago.Activo ?? existingMetodoPago.Activo;

            await _metodoPagoService.UpdateMetodoPagoAsync(existingMetodoPago);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetodoPago(int id)
        {
            var success = await _metodoPagoService.DeleteMetodoPagoAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
