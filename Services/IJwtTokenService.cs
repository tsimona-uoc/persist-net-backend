using System.Security.Claims;
using persist_net_backend.Models;

namespace persist_net_backend.Services
{
    public interface IJwtTokenService
    {
        /// <summary>
        /// Genera un token JWT para un usuario
        /// </summary>
        /// <param name="userId">El ID del usuario</param>
        /// <param name="email">El email del usuario</param>
        /// <returns>Un token JWT válido</returns>
        string GenerateToken(Guid userId, string email);

        /// <summary>
        /// Valida un token JWT y retorna los claims si es válido
        /// </summary>
        /// <param name="token">El token JWT a validar</param>
        /// <returns>Una colección de claims si es válido, null si no es válido</returns>
        ClaimsPrincipal? ValidateToken(string token);

        /// <summary>
        /// Extrae el ID del usuario desde un token JWT
        /// </summary>
        /// <param name="token">El token JWT</param>
        /// <returns>El ID del usuario, o Empty Guid si no se puede extraer</returns>
        Guid GetUserIdFromToken(string token);

        /// <summary>
        /// Actualiza la sesión del usuario con el nuevo token JWT
        /// </summary>
        /// <param name="userId">El ID del usuario</param>
        /// <returns>La sesión actualizada del usuario</returns>
        Task<UserSession> UpdateUserSession(Guid userId);
    }
}
