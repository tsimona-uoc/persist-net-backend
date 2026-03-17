using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Hotel;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        private HotelResponse MapToResponse(Hotel hotel)
        {
            return new HotelResponse
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Description = hotel.Description,
                Address = hotel.Address,
                PhoneNumber = hotel.PhoneNumber,
                Email = hotel.Email
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelResponse>> GetHotel(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
                return NotFound();

            return Ok(MapToResponse(hotel));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelResponse>>> GetAllHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<HotelResponse>> CreateHotel([FromBody] CreateHotelRequest hotel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdHotel = await _hotelService.CreateHotelAsync(new Hotel
            {
                Name = hotel.Name,
                Description = hotel.Description ?? string.Empty,
                Address = hotel.Address ?? string.Empty,
                PhoneNumber = hotel.PhoneNumber ?? string.Empty,
                Email = hotel.Email ?? string.Empty,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });
            return CreatedAtAction(nameof(GetHotel), new { id = createdHotel.Id }, MapToResponse(createdHotel));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelRequest hotel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingHotel = await _hotelService.GetHotelByIdAsync(id);
            if (existingHotel == null)
                return NotFound();

            existingHotel.Name = hotel.Name ?? existingHotel.Name;
            existingHotel.Description = hotel.Description ?? existingHotel.Description;
            existingHotel.Address = hotel.Address ?? existingHotel.Address;
            existingHotel.PhoneNumber = hotel.PhoneNumber ?? existingHotel.PhoneNumber;
            existingHotel.Email = hotel.Email ?? existingHotel.Email;
            existingHotel.LastModifiedBy = "system";
            existingHotel.LastModifiedAt = DateTime.Now;

            await _hotelService.UpdateHotelAsync(existingHotel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var success = await _hotelService.DeleteHotelAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
