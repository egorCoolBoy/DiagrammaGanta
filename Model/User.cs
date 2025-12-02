namespace Diagramma_Ganta.Model;

public class User
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public List<Team>Teams { get; set; }
    public List<Task>Tasks { get; set; }
}