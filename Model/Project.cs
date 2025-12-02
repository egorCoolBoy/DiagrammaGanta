namespace Diagramma_Ganta.Model;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationDate { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    
    public Guid? OwnerId { get; set; }
    public Guid  SubjectId { get; set; }

    public List<Task> Tasks { get; set; } = new List<Task>();
    public Subject Subject { get; set; }
    public Team? Team { get; set; }     
    public User Owner { get; set; }
    
}