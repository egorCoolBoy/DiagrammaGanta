namespace Diagramma_Ganta.Dto.Project;

public class GetProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string OwnerName { get; set; }
    
}


