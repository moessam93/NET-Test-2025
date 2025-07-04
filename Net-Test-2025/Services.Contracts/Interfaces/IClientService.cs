using Net_Test_2025.Domains;
using Net_Test_2025.Services.Contracts.DTOs;

namespace Net_Test_2025.Services.Contracts.Interfaces;

public interface IClientService
{
    Task<ServiceResult> GetAllClients(ClientsListingRequest request);
    Task<ServiceResult> GetClientById(int id);
    Task<ServiceResult> CreateClient(CreateClientRequest request);
    Task<ServiceResult> UpdateClient(UpdateClientDto request, int id);
    Task<ServiceResult> DeleteClient(int id);
}
