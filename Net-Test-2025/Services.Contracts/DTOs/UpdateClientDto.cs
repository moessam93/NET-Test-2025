using Net_Test_2025.Helpers;
using Net_Test_2025.Services.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;
namespace Net_Test_2025.Services.Contracts.DTOs;

public class UpdateClientDto : IValidatable
{
    public string? Name { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    [Phone]
    public string? Phone { get; set; }
    [Range(1, 100)]
    public string? Age { get; set; }
    
    [ValidGender]
    public string? Gender { get; set; }
}
