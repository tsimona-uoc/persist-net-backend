using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Habitacion>> CreateHabitacion([FromBody] Habitacion habitacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdHabitacion = await _habitacionService.CreateHabitacionAsync(habitacion);
            return CreatedAtAction(nameof(GetHabitacion), new { id = createdHabitacion.Id }, createdHabitacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabitacion(int id, [FromBody] Habitacion habitacion)
        {
            if (id != habitacion.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingHabitacion = await _habitacionService.GetHabitacionByIdAsync(id);
            if (existingHabitacion == null)
                return NotFound();

            await _habitacionService.UpdateHabitacionAsync(habitacion);
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
