using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto;

public class LoginDto
{
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Невалидный формат email")]
    public string email { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    public string password { get; set; }
    
}