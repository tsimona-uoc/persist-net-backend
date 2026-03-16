using persist_net_backend.DTOs.User;

namespace persist_net_backend.DTOs.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRoleResponse? UserRole { get; set; }
    }
}
