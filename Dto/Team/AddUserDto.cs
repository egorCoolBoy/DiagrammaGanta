using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto.Team;

public class AddUserInTeamDto
{
    public Guid ProjectId { get; set; }
    [EmailAddress(ErrorMessage = "Невалидный формат email")]
    public string Email { get; set; }
}