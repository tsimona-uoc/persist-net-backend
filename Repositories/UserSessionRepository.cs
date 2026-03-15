using System.IdentityModel.Tokens.Jwt;
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
            // Decodificar JWT para obtener la fecha de expiración real
            var handler = new JwtSecurityTokenHandler();
            DateTime expiresAt = DateTime.Now.AddHours(24); // Fallback por si acaso
            
            try
            {
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                    if (jwtToken?.ValidTo != null)
                    {
                        // Convertir ValidTo de UTC a hora local
                        expiresAt = jwtToken.ValidTo.ToLocalTime();
                    }
                }
            }
            catch
            {
                // Si ocurre un error al decodificar el token, se usará la fecha de expiración por defecto
            }

            var session = await _context.UserSessions
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (session == null)
            {
                session = new UserSession
                {
                    UserId = userId,
                    Token = token,
                    CreatedAt = DateTime.Now,
                    ExpiresAt = expiresAt,
                    IpAddress = string.Empty,
                    LastModifiedBy = "system",
                    LastModifiedAt = DateTime.Now
                };
                _context.UserSessions.Add(session);
            }
            else
            {
                session.Token = token;
                session.CreatedAt = DateTime.Now;
                session.ExpiresAt = expiresAt;
                session.LastModifiedBy = "system";
                session.LastModifiedAt = DateTime.Now;
                _context.UserSessions.Update(session);
            }

            await _context.SaveChangesAsync();
            return session;
        }
    }
}