namespace Diagramma_Ganta.Dto.Project;

public class CreateProjectDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid SubjectId { get; set; }
}