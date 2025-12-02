
using TaskStatus = Diagramma_Ganta.Model.TaskStatus;

namespace Diagramma_Ganta.Dto.Task;

public class TaskDto
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime? Deadline { get; set; }
    
    public UserDtoOut Author { get; set; }
    public List<UserDtoOut> Users { get; set; } = new List<UserDtoOut>();
    public List<TaskPredecessorDto> Predecessors { get; set; } = new List<TaskPredecessorDto>();
}

public class TaskPredecessorDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}

public class UserDtoOut
{
    public Guid Id { get; set; }
    public string Email { get; set; }
}

