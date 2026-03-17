using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Reserva;
using persist_net_backend.DTOs.Regimen;
using persist_net_backend.DTOs.EstadoReserva;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        private RegimenResponse MapRegimenToResponse(Regimen regimen)
        {
            return new RegimenResponse
            {
                Id = regimen.Id,
                Nombre = regimen.Nombre,
                Descripcion = regimen.Descripcion,
                Activo = regimen.Activo
            };
        }

        private EstadoReservaResponse MapEstadoReservaToResponse(EstadoReserva estado)
        {
            return new EstadoReservaResponse
            {
                Id = estado.Id,
                Nombre = estado.Nombre,
                Descripcion = estado.Descripcion,
                Activo = estado.Activo
            };
        }

        private ReservaResponse MapToResponse(Reserva reserva)
        {
            return new ReservaResponse
            {
                Id = reserva.Id,
                ClienteId = reserva.ClienteId,
                HabitacionId = reserva.HabitacionId,
                FechaEntrada = reserva.FechaEntrada,
                FechaSalida = reserva.FechaSalida,
                Regimen = reserva.Regimen != null ? MapRegimenToResponse(reserva.Regimen) : throw new InvalidOperationException("Regimen is required"),
                EstadoReserva = reserva.EstadoReserva != null ? MapEstadoReservaToResponse(reserva.EstadoReserva) : throw new InvalidOperationException("EstadoReserva is required"),
                PrecioActual = reserva.PrecioActual,
                FechaCreacion = reserva.FechaCreacion
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaResponse>> GetReserva(int id)
        {
            var reserva = await _reservaService.GetReservaByIdAsync(id);
            if (reserva == null)
                return NotFound();

            return Ok(MapToResponse(reserva));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaResponse>>> GetAllReservas()
        {
            var reservas = await _reservaService.GetAllReservasAsync();
            return Ok(reservas.Select(MapToResponse));
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<ReservaResponse>>> GetReservasByCliente(int clienteId)
        {
            var reservas = await _reservaService.GetReservasByClienteAsync(clienteId);
            return Ok(reservas.Select(MapToResponse));
        }

        [HttpGet("habitacion/{habitacionId}")]
        public async Task<ActionResult<IEnumerable<ReservaResponse>>> GetReservasByHabitacion(int habitacionId)
        {
            var reservas = await _reservaService.GetReservasByHabitacionAsync(habitacionId);
            return Ok(reservas.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<ReservaResponse>> CreateReserva([FromBody] CreateReservaRequest reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdReserva = await _reservaService.CreateReservaAsync(new Reserva
            {
                ClienteId = reserva.ClienteId,
                HabitacionId = reserva.HabitacionId,
                FechaEntrada = reserva.FechaEntrada,
                FechaSalida = reserva.FechaSalida,
                RegimenId = reserva.RegimenId,
                EstadoReservaId = reserva.EstadoReservaId,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
            return CreatedAtAction(nameof(GetReserva), new { id = createdReserva.Id }, MapToResponse(createdReserva));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] UpdateReservaRequest reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingReserva = await _reservaService.GetReservaByIdAsync(id);
            if (existingReserva == null)
                return NotFound();

            existingReserva.ClienteId = reserva.ClienteId ?? existingReserva.ClienteId;
            existingReserva.HabitacionId = reserva.HabitacionId ?? existingReserva.HabitacionId;
            existingReserva.FechaEntrada = reserva.FechaEntrada ?? existingReserva.FechaEntrada;
            existingReserva.FechaSalida = reserva.FechaSalida ?? existingReserva.FechaSalida;
            existingReserva.RegimenId = reserva.RegimenId ?? existingReserva.RegimenId;
            existingReserva.EstadoReservaId = reserva.EstadoReservaId ?? existingReserva.EstadoReservaId;
            existingReserva.LastModifiedBy = "system";
            existingReserva.LastModifiedAt = DateTime.Now;

            await _reservaService.UpdateReservaAsync(existingReserva);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var success = await _reservaService.DeleteReservaAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
