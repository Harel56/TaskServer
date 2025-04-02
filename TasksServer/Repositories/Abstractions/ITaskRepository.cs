using TasksServer.Models;

namespace TasksServer.Repositories.Abstractions;

public interface ITaskRepository
{
    void AddTask(TaskItem newTask);
    TaskItem? GetTask(string id);
    void DeleteTask(string id);
    IEnumerable<TaskItem> GetUserTasks(string userId);
}
