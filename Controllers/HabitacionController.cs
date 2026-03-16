using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.EstadoHabitacion;
using persist_net_backend.DTOs.Habitacion;
using persist_net_backend.DTOs.TipoHabitacion;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HabitacionController : ControllerBase
    {
        private readonly IHabitacionService _habitacionService;

        public HabitacionController(IHabitacionService habitacionService)
        {
            _habitacionService = habitacionService;
        }

        private TipoHabitacionResponse MapTipoHabitacionToResponse(TipoHabitacion tipoHabitacion)
        {
            return new TipoHabitacionResponse
            {
                Id = tipoHabitacion.Id,
                Nombre = tipoHabitacion.Nombre,
                Descripcion = tipoHabitacion.Descripcion
            };
        }

        private EstadoHabitacionResponse MapEstadoHabitacionToResponse(EstadoHabitacion estadoHabitacion)
        {
            return new EstadoHabitacionResponse
            {
                Id = estadoHabitacion.Id,
                Nombre = estadoHabitacion.Nombre,
                Descripcion = estadoHabitacion.Descripcion
            };
        }

        private HabitacionResponse MapToResponse(Habitacion habitacion)
        {
            return new HabitacionResponse
            {
                Id = habitacion.Id,
                HotelId = habitacion.HotelId,
                Planta = habitacion.Planta,
                Numero = habitacion.Numero,
                TipoHabitacion = habitacion.TipoHabitacion != null ? MapTipoHabitacionToResponse(habitacion.TipoHabitacion) : throw new InvalidOperationException("TipoHabitacion is required"),
                EstadoHabitacion = habitacion.EstadoHabitacion != null ? MapEstadoHabitacionToResponse(habitacion.EstadoHabitacion) : throw new InvalidOperationException("EstadoHabitacion is required")
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HabitacionResponse>> GetHabitacion(int id)
        {
            var habitacion = await _habitacionService.GetHabitacionByIdAsync(id);
            if (habitacion == null)
                return NotFound();

            return Ok(MapToResponse(habitacion));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HabitacionResponse>>> GetAllHabitaciones()
        {
            var habitaciones = await _habitacionService.GetAllHabitacionesAsync();
            return Ok(habitaciones.Select(MapToResponse));
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<HabitacionResponse>>> GetHabitacionesByHotel(int hotelId)
        {
            var habitaciones = await _habitacionService.GetHabitacionesByHotelAsync(hotelId);
            return Ok(habitaciones.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<HabitacionResponse>> CreateHabitacion([FromBody] CreateHabitacionRequest habitacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdHabitacion = await _habitacionService.CreateHabitacionAsync(new Habitacion
            {
                HotelId = habitacion.HotelId,
                TipoHabitacionId = habitacion.TipoHabitacionId,
                Planta = habitacion.Planta,
                Numero = habitacion.Numero,
                EstadoHabitacionId = habitacion.EstadoHabitacionId,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
            return CreatedAtAction(nameof(GetHabitacion), new { id = createdHabitacion.Id }, MapToResponse(createdHabitacion));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabitacion(int id, [FromBody] UpdateHabitacionRequest habitacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingHabitacion = await _habitacionService.GetHabitacionByIdAsync(id);
            if (existingHabitacion == null)
                return NotFound();

            existingHabitacion.HotelId = habitacion.HotelId ?? existingHabitacion.HotelId;
            existingHabitacion.TipoHabitacionId = habitacion.TipoHabitacionId ?? existingHabitacion.TipoHabitacionId;
            existingHabitacion.Planta = habitacion.Planta ?? existingHabitacion.Planta;
            existingHabitacion.Numero = habitacion.Numero ?? existingHabitacion.Numero;
            existingHabitacion.EstadoHabitacionId = habitacion.EstadoHabitacionId ?? existingHabitacion.EstadoHabitacionId;
            existingHabitacion.LastModifiedBy = "system";
            existingHabitacion.LastModifiedAt = DateTime.Now;

            await _habitacionService.UpdateHabitacionAsync(existingHabitacion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitacion(int id)
        {
            var success = await _habitacionService.DeleteHabitacionAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
