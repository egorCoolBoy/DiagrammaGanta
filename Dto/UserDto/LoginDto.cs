using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string password { get; set; }
    
}