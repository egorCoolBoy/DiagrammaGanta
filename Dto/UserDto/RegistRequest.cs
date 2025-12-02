using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto;

public class RegistRequest
{
    [Required(ErrorMessage = "Email обязателен")]
    [RegularExpression(
        @"^(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|\""(?:(?:\\[\x01-\x09\x0b\x0c\x0e-\x7f])|[^\""])*\"")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}|(?:\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-zA-Z0-9-]*[a-zA-Z0-9]:+(?:[\x01-\x08\x0b\x0c\x0e-\x7f])+)\]))$"
    ,ErrorMessage = "Невалидный email")]
    public string email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(8, ErrorMessage = "Пароль должен содержать минимум 8 символом")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "Минимум 1 буква или 1 число")]
    public string password { get; set; } = string.Empty;
    
}