using TasksServer.Repositories.Abstractions;

namespace TasksServer.Repositories.InMemory;

public static class ServiceCollectionExtentions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, UsersRepository>();
        services.AddSingleton<ITaskRepository, TaskRepository>();
    }
}
