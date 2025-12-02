using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto.Team;

public class DeleteUserDto
{
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public Guid UserId { get; set; }
}