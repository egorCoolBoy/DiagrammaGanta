using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Model;

public class Team
{
    [Key]
    public Guid ProjectId { get; set; }

    public List<User> Users { get; set; } = new List<User>();
}