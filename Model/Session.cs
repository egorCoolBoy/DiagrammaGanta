using System.ComponentModel.DataAnnotations;

namespace Diagramma_Ganta.Model;

public class Session
{
    [Key]
    public Guid Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; }
}