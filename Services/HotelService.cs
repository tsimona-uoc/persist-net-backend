using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _hotelRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _hotelRepository.GetAllAsync();
        }

        public async Task<Hotel> CreateHotelAsync(Hotel hotel)
        {
            return await _hotelRepository.AddAsync(hotel);
        }

        public async Task<Hotel> UpdateHotelAsync(Hotel hotel)
        {
            return await _hotelRepository.UpdateAsync(hotel);
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            return await _hotelRepository.DeleteAsync(id);
        }
    }
}
