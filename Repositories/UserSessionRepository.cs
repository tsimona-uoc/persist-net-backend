using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;

namespace persist_net_backend.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly AppDbContext _context;

        public UserSessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserSession> UpdateSessionAsync(Guid userId, string token)
        {
            var session = await _context.UserSessions
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (session == null)
            {
                session = new UserSession
                {
                    UserId = userId,
                    JwtToken = token,
                    LastModifiedBy = "system",
                    LastModifiedAt = DateTime.UtcNow
                };
                _context.UserSessions.Add(session);
            }
            else
            {
                session.JwtToken = token;
                session.LastModifiedBy = "system";
                session.LastModifiedAt = DateTime.UtcNow;
                _context.UserSessions.Update(session);
            }

            await _context.SaveChangesAsync();
            return session;
        }
    }
}