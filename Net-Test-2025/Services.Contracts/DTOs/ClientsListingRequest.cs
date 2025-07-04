namespace Net_Test_2025.Services.Contracts.DTOs;

public class ClientsListingRequest
{
    public string? SearchTerm { get; set; }
    public string? Gender { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

