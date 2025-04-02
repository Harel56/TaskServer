

namespace TasksServer.Models
{
    public class User
    {
        public required string Username { get; set; }
        public required string Id { get; set; }
        public required string PasswordHash { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
