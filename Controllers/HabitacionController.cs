using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Habitacion;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Habitacion>> GetHabitacion(int id)
        {
            var habitacion = await _habitacionService.GetHabitacionByIdAsync(id);
            if (habitacion == null)
                return NotFound();

            return Ok(habitacion);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Habitacion>>> GetAllHabitaciones()
        {
            var habitaciones = await _habitacionService.GetAllHabitacionesAsync();
            return Ok(habitaciones);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<Habitacion>>> GetHabitacionesByHotel(int hotelId)
        {
            var habitaciones = await _habitacionService.GetHabitacionesByHotelAsync(hotelId);
            return Ok(habitaciones);
        }

        [HttpPost]
        public async Task<ActionResult<Habitacion>> CreateHabitacion([FromBody] CreateHabitacionRequest habitacion)
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
            return CreatedAtAction(nameof(GetHabitacion), new { id = createdHabitacion.Id }, createdHabitacion);
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
