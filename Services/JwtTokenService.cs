using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using persist_net_backend.Models;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public JwtTokenService(IUserSessionRepository userSessionRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _userSessionRepository = userSessionRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
                ?? _configuration["Jwt:SecretKey"] 
                ?? throw new InvalidOperationException("JWT_SECRET_KEY no está configurado");
            
            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
                ?? _configuration["Jwt:Issuer"] 
                ?? "persist-net-backend";
            
            _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                ?? _configuration["Jwt:Audience"] 
                ?? "persist-net-users";
            
            _expirationMinutes = int.TryParse(
                Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES") 
                ?? _configuration["Jwt:ExpirationMinutes"],
                out var minutes) ? minutes : 60;
        }

        public string GenerateToken(Guid userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("sub", userId.ToString()),
                new Claim("iat", DateTimeOffset.Now.ToUnixTimeSeconds().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }


        public Guid GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            if (principal == null)
            {
                return Guid.Empty;
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) 
                ?? principal.FindFirst("sub");
            
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Actualiza la sesión del usuario con el nuevo token JWT
        /// </summary>
        /// <param name="userId">El ID del usuario</param>
        /// <param name="token">El nuevo token JWT</param>
        /// <param name="address">La dirección IP del usuario</param>
        /// <returns>La sesión actualizada del usuario</returns>
        public async Task<UserSession> UpdateUserSession(Guid userId, string address)
        {
            var user = await this._userRepository.findById(userId); // Verificar que el usuario existe antes de actualizar la sesión

            if (user == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            string token = GenerateToken(userId, user.Email);
            var userSession = await _userSessionRepository.UpdateSessionAsync(userId, token, address);
            return userSession;
        }
    }
}
