using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto;

public class MeDto
{
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string password { get; set; } = string.Empty;
}