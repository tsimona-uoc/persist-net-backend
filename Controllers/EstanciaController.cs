using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Estancia;
using persist_net_backend.DTOs.EstadoEstancia;
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

        private EstadoEstanciaResponse MapEstadoEstanciaToResponse(EstadoEstancia estado)
        {
            return new EstadoEstanciaResponse
            {
                Id = estado.Id,
                Nombre = estado.Nombre,
                Descripcion = estado.Descripcion,
                Activo = estado.Activo
            };
        }

        private EstanciaResponse MapToResponse(Estancia estancia)
        {
            return new EstanciaResponse
            {
                Id = estancia.Id,
                ReservaId = estancia.ReservaId,
                FechaCheckIn = estancia.FechaCheckIn,
                FechaCheckOut = estancia.FechaCheckOut,
                EstadoEstancia = estancia.EstadoEstancia != null ? MapEstadoEstanciaToResponse(estancia.EstadoEstancia) : throw new InvalidOperationException("EstadoEstancia is required")
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstanciaResponse>> GetEstancia(int id)
        {
            var estancia = await _estanciaService.GetEstanciaByIdAsync(id);
            if (estancia == null)
                return NotFound();

            return Ok(MapToResponse(estancia));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstanciaResponse>>> GetAllEstancias()
        {
            var estancias = await _estanciaService.GetAllEstanciasAsync();
            return Ok(estancias.Select(MapToResponse));
        }

        [HttpGet("reserva/{reservaId}")]
        public async Task<ActionResult<IEnumerable<EstanciaResponse>>> GetEstanciasByReserva(int reservaId)
        {
            var estancias = await _estanciaService.GetEstanciasByReservaAsync(reservaId);
            return Ok(estancias.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<EstanciaResponse>> CreateEstancia([FromBody] CreateEstanciaRequest estancia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdEstancia = await _estanciaService.CreateEstanciaAsync(new Estancia
                {
                    ReservaId = estancia.ReservaId,
                    FechaCheckIn = estancia.FechaCheckIn,
                    FechaCheckOut = estancia.FechaCheckOut,
                    EstadoEstanciaId = estancia.EstadoEstanciaId,
                    LastModifiedBy = "system",
                    LastModifiedAt = DateTime.Now
                });

                return CreatedAtAction(nameof(GetEstancia), new { id = createdEstancia.Id }, MapToResponse(createdEstancia));
            }
            catch (EntityReferenceValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstancia(int id, [FromBody] UpdateEstanciaRequest estancia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEstancia = await _estanciaService.GetEstanciaByIdAsync(id);
            if (existingEstancia == null)
                return NotFound();

            await _estanciaService.UpdateEstanciaAsync(new Estancia
            {
                Id = id,
                ReservaId = estancia.ReservaId ?? existingEstancia.ReservaId,
                FechaCheckIn = estancia.FechaCheckIn ?? existingEstancia.FechaCheckIn,
                FechaCheckOut = estancia.FechaCheckOut ?? existingEstancia.FechaCheckOut,
                EstadoEstanciaId = estancia.EstadoEstanciaId ?? existingEstancia.EstadoEstanciaId,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
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
