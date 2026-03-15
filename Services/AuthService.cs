using persist_net_backend.Models;
using persist_net_backend.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace persist_net_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IJwtTokenService jwtTokenService)
        {
            this._userRepository = userRepository;
            this._userRoleRepository = userRoleRepository;
            this._jwtTokenService = jwtTokenService;
        }

        
        /// <summary>
        /// Intenta autenticar a un usuario con el email y password proporcionados.
        /// Retorna un tuple indicando si la autenticación fue exitosa y un token JWT (vacío si no fue exitosa).
        /// </summary>
        /// <param name="email">El email del usuario</param>
        /// <param name="password">El password del usuario</param>
        /// <param name="address">La dirección IP del usuario</param>
        /// <returns>Un tuple indicando si la autenticación fue exitosa y un token JWT (vacío si no fue exitosa)</returns>
        public async Task<(bool success, string token)> LoginAsync(string email, string password, string address)
        {
            // Buscar el usuario por email
            var user = await _userRepository.findByEmail(email);
            
            if (user == null)
            {
                return (false, string.Empty);
            }
            
            // Calcular el hash SHA-256 del password recibido
            var hashedPassword = ComputeSha256Hash(password);
            
            // Comparar el hash del password con el almacenado en la base de datos
            if (hashedPassword == user.PasswordHash)
            {
                // Actualizar la sesión del usuario en BBDD
                UserSession result = await this._jwtTokenService.UpdateUserSession(user.Id, address);
                return (true, result.Token);
            }
            
            return (false, string.Empty);
        }

        /// <summary>
        /// Intenta registrar a un nuevo usuario con los datos proporcionados.
        /// Retorna un tuple indicando si el registro fue exitoso y un token JWT (vacío si no fue exitoso).
        /// </summary> 
        /// <param name="name">El nombre del usuario</param>
        /// <param name="surname">El apellido del usuario</param>
        /// <param name="email">El email del usuario</param>
        /// <param name="password">El password del usuario</param>
        /// <
        public async Task<bool> RegisterAsync(string name, string surname, string email, string password, string role){

            // Validar que el rol proporcionado es válido
            var userRole = await this._userRoleRepository.GetByCode(role);
            if (userRole == null)
            {
                return false;
            }

            // Verificar si el email ya está registrado
            var existingUser = await _userRepository.findByEmail(email);
            if (existingUser != null)
            {
                return false;
            }

            // Calcular el hash SHA-256 del password
            var hashedPassword = ComputeSha256Hash(password);

            // Crear un nuevo usuario
            var newUser = new Models.User
            {
                Name = name,
                Surname = surname,
                Email = email,
                PasswordHash = hashedPassword,
                UserRoleId = userRole.Id,
                LastModifiedAt = DateTime.Now,
                LastModifiedBy = "system"
            };

            // Guardar el usuario en la base de datos
            await _userRepository.Add(newUser);
            return true;
        }
        
        /// <summary>
        /// Calcula el hash SHA-256 de una cadena
        /// </summary>
        /// <param name="input">La cadena a hashear</param>
        /// <returns>El hash SHA-256 de la cadena, codificado en Base64</returns>
        private string ComputeSha256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}