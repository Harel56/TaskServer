using TasksServer.Models;
using TasksServer.Repositories.Abstractions;

namespace TasksServer.Repositories.InMemory;

public class UsersRepository: IUserRepository
{
    private Dictionary<string, User> users = [];

    public User? GetUser(string username)
    {
        return users.GetValueOrDefault(username);
    }

    public void AddUser(User user) {
        users.Add(user.Username, user);
    }

    public User? GetUserByRefreshToken(string refreshToken)
    {
        return users.Values.FirstOrDefault(x => x.RefreshToken == refreshToken);
    }
}
