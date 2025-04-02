using TasksServer.Models;
using TasksServer.Repositories.Abstractions;

namespace TasksServer.Repositories.InMemory;

public class TaskRepository: ITaskRepository
{
    private readonly List<TaskItem> _tasks = [];

    public void AddTask(TaskItem newTask)
    {
        _tasks.Add(newTask);
    }

    public TaskItem? GetTask(string id)
    {
        var task = _tasks.FirstOrDefault(x => x.Id == id);
        return task;
    }

    public void DeleteTask(string id)
    {
        var task = _tasks.FirstOrDefault(x => x.Id == id);
        if (task != null)
        {
            _tasks.Remove(task);
        }
    }

    public IEnumerable<TaskItem> GetUserTasks(string userId)
    {
        var tasks = _tasks.Where(x => x.UserId == userId);
        return tasks;
    }
}
