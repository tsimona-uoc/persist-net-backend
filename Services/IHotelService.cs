using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IHotelService
    {
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel> CreateHotelAsync(Hotel hotel);
        Task<Hotel> UpdateHotelAsync(Hotel hotel);
        Task<bool> DeleteHotelAsync(int id);
    }
}
