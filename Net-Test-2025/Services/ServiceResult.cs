using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace Net_Test_2025.Services.Contracts.DTOs; 

public class ServiceResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public object? Data { get; set; }

    public static ServiceResult Success(object? data = null)
    {
        return new ServiceResult { IsSuccess = true, Data = data };
    }

    public static ServiceResult Error(string errorMessage)
    {
        return new ServiceResult { IsSuccess = false, ErrorMessage = errorMessage };
    }

    public static ServiceResult BadRequest(string errorMessage)
    {
        return new ServiceResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}

public static class ServiceError
{
    public static ServiceResult BadRequest(string message)
    {
        return ServiceResult.BadRequest(message);
    }

    public static ServiceResult NotFound(string message)
    {
        return ServiceResult.Error(message);
    }

    public static ServiceResult Error(string message)
    {
        return ServiceResult.Error(message);
    }

    public static ServiceResult BadRequest(List<ValidationResult> validationResults)
    {
        return ServiceResult.BadRequest(string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
    }
} 