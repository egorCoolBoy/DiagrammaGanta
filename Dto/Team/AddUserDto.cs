using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto.Team;

public class AddUserInTeamDto
{
    public Guid ProjectId { get; set; }
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
}