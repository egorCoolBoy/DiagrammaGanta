namespace Diagramma_Ganta.Model;

public class TaskDependency
{
    public Guid TaskId { get; set; }
    public Guid PredecessorId { get; set; }
    
    public Task Predecessor { get; set; }
}