using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto.Task;

public class UpdateTaskDto
{
    [StringLength(100, ErrorMessage = "Title too long")]
    public string? Title { get; set; }

    [StringLength(1000, ErrorMessage = "Description too long")]
    public string? Description { get; set; }

    public TaskStatus? TaskStatus { get; set; }

    [FutureDate(ErrorMessage = "Deadline must be in the future")]
    public DateTime? Deadline { get; set; }

    public List<UserIdDto>? Users { get; set; }
    public List<Guid>? Predecessors { get; set; }
}

public class UserIdDto
{
    public Guid Id { get; set; }
}


public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
            return date > DateTime.UtcNow;
        return true; 
    }
}