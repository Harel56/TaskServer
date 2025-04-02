namespace TasksServer.Models
{
    public class TaskItem : TaskItemInput
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }

    }
}
