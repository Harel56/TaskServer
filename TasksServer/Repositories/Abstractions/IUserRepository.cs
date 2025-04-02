using TasksServer.Models;

namespace TasksServer.Repositories.Abstractions
{
    public interface IUserRepository
    {
        User? GetUser(string username);
        void AddUser(User user);
        User? GetUserByRefreshToken(string refreshToken);
    }
}
