using Net_Test_2025.Helpers;
using Net_Test_2025.Services.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;
namespace Net_Test_2025.Services.Contracts.DTOs;

public class CreateClientRequest : IValidatable
{
    public required string Name { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    [Phone]
    public required string Phone { get; set; }
    [Range(1, 100)]
    public string? Age { get; set; }
    
    [ValidGender]
    public required string Gender { get; set; }
}
