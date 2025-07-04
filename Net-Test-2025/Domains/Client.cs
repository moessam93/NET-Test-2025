using System.ComponentModel.DataAnnotations;
using Net_Test_2025.Helpers;

namespace Net_Test_2025.Domains;

public class Client
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Age { get; set; }
    public required Gender Gender { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public bool Deleted { get; set; } = false;
}
