using Diagramma_Ganta.Dto.Team;
using Diagramma_Ganta.Dto.Task;

namespace Diagramma_Ganta.Dto.Project;

public class ComeInProjectDto
{ 
    public string Title { get; set; } 
    public string Description { get; set; } 
    public string Subject { get; set; }
    public TeamDto Team { get; set; }
    public string OwnerName { get; set; }
    public List<TaskDto> Tasks { get; set; }
}