using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksServer.Models;
using TasksServer.Repositories.Abstractions;

namespace TasksServer.Controllers;

[Route("tasks")]
[Authorize]
[ApiController]
public class TasksController(ITaskRepository taskRepository) : ControllerBase
{
    [HttpGet]
    public IEnumerable<TaskItem> GetTasks()
    {
        var userId = (User.FindFirst("id")?.Value) ?? throw new Exception("No id claim found");
        var userTasks = taskRepository.GetUserTasks(userId);
        return userTasks;
    }

    [HttpPost]
    public TaskItem CreateTask([FromBody] TaskItemInput newTask)
    {
        var userId = (User.FindFirst("id")?.Value) ?? throw new Exception("No id claim found");
        var task = new TaskItem
        {
            Title = newTask.Title,
            Description = newTask.Description,
            Id = Guid.NewGuid().ToString(),
            UserId = userId
        };
        taskRepository.AddTask(task);
        return task;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTask(string id, [FromBody] TaskItemInput updatedTask)
    {
        var userId = User.FindFirst("id")?.Value;
        var task = taskRepository.GetTask(id);
        if (task == null || task.UserId != userId)
        {
            return NotFound();
        }
        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(string id)
    {
        var userId = User.FindFirst("id")?.Value;
        var task = taskRepository.GetTask(id);
        if (task == null || task.UserId != userId)
        {
            return NotFound();
        }
        taskRepository.DeleteTask(id);
        return Ok();
    }

}
