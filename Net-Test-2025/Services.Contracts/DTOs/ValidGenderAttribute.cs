using Net_Test_2025.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Net_Test_2025.Services.Contracts.DTOs;

public class ValidGenderAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return true;
        
        if (value is string genderString)
        {
            return Enum.TryParse<Gender>(genderString, true, out _);
        }
        
        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The {name} field must be a valid gender value (Male or Female).";
    }
} 