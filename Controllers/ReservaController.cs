using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Reserva;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _reservaService.GetReservaByIdAsync(id);
            if (reserva == null)
                return NotFound();

            return Ok(reserva);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetAllReservas()
        {
            var reservas = await _reservaService.GetAllReservasAsync();
            return Ok(reservas);
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasByCliente(int clienteId)
        {
            var reservas = await _reservaService.GetReservasByClienteAsync(clienteId);
            return Ok(reservas);
        }

        [HttpGet("habitacion/{habitacionId}")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasByHabitacion(int habitacionId)
        {
            var reservas = await _reservaService.GetReservasByHabitacionAsync(habitacionId);
            return Ok(reservas);
        }

        [HttpPost]
        public async Task<ActionResult<Reserva>> CreateReserva([FromBody] CreateReservaRequest reserva)
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
            return CreatedAtAction(nameof(GetReserva), new { id = createdReserva.Id }, createdReserva);
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
