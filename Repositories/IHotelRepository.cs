using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetByIdAsync(int id);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel> AddAsync(Hotel hotel);
        Task<Hotel> UpdateAsync(Hotel hotel);
        Task<bool> DeleteAsync(int id);
    }
}
