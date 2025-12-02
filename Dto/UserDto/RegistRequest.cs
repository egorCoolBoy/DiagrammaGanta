using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Dto;

public class RegistRequest
{
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(
        @"^(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|\""(?:(?:\\[\x01-\x09\x0b\x0c\x0e-\x7f])|[^\""])*\"")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}|(?:\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-zA-Z0-9-]*[a-zA-Z0-9]:+(?:[\x01-\x08\x0b\x0c\x0e-\x7f])+)\]))$"
    ,ErrorMessage = "Invalid email format")]
    public string email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "Password must be at least 1 letter and 1 number")]
    public string password { get; set; } = string.Empty;
    
}