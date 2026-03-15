using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;

        public UserRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserRole?> GetById(int id)
        {
            return await _context.UserRoles.FindAsync(id);
        }

        public async Task<UserRole?> GetByCode(string code)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            return await _context.UserRoles.ToListAsync();
        }

        public async Task<UserRole> Add(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }

        public async Task Update(UserRole userRole)
        {
            _context.UserRoles.Update(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var userRole = await GetById(id);
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }
    }
}
