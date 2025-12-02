namespace Diagramma_Ganta.Model;

public class Task
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime? Deadline { get; set; }
    
    public Guid AuthorTaskId { get; set; }
    public Guid ProjectId { get; set; }
    
    public Project Project { get; set; }
    public User AuthorTask { get; set; }
    public List<User> Users { get; set; } = new List<User>();
    public List<TaskDependency> Dependencies { get; set; } = new List<TaskDependency>();
}