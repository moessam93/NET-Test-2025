using System.ComponentModel.DataAnnotations;

namespace Net_Test_2025.Services.Contracts.Interfaces;

public interface IValidatable
{
}

public static class ValidatableExtensions
{
    public static List<ValidationResult> Validate(this IValidatable validatable)
    {
        var validationContext = new ValidationContext(validatable);
        var validationResults = new List<ValidationResult>();
        
        Validator.TryValidateObject(validatable, validationContext, validationResults, validateAllProperties: true);
        
        return validationResults;
    }

    public static bool IsValid(this IValidatable validatable)
    {
        return validatable.Validate().Count == 0;
    }
} 