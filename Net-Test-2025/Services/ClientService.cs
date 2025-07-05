using Microsoft.EntityFrameworkCore;
using Net_Test_2025.Data;
using Net_Test_2025.Domains;
using Net_Test_2025.Helpers;
using Net_Test_2025.Services.Contracts.DTOs;
using Net_Test_2025.Services.Contracts.Interfaces;
using System.Globalization;
using System.Text;

namespace Net_Test_2025.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;
    private readonly TextInfo _textInfo;

    public ClientService(AppDbContext context)
    {
        _context = context;
        _textInfo = CultureInfo.InvariantCulture.TextInfo;
    }
    
    public async Task<ServiceResult> GetAllClients(ClientsListingRequest request)
    {
        try
        {
            var query = _context.Clients.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(request.SearchTerm));
            }
            
            if(!string.IsNullOrWhiteSpace(request.Gender))
            {
                query = query.Where(c => c.Gender.ToString().ToLower() == request.Gender.ToLower());
            }

            var clients = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var result = clients.Select(c => new GetClientDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Age = c.Age ?? "N/A",
                Gender = c.Gender.ToString(),
                CreatedAt = c.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
            }).ToList();

            return ServiceResult.Success(result);
        }
        catch (Exception ex)
        {
            return ServiceResult.Error(ex.Message);
        }
    }

    public async Task<ServiceResult> GetClientById(int id)
    {
        try
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
            {
                return ServiceError.NotFound("Client not found");
            }
            
            var result = new GetClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                Age = client.Age ?? "N/A",
                Gender = client.Gender.ToString(),
                CreatedAt = client.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
            };

            return ServiceResult.Success(result);
        }
        catch (Exception ex)
        {
            return ServiceResult.Error(ex.Message);
        }
    }
    
    public async Task<ServiceResult> CreateClient(CreateClientRequest request)
    {
        try
        {
            var clientFromDb = await _context.Clients.FirstOrDefaultAsync(c => c.Email == request.Email || c.Phone == request.Phone);
            if (clientFromDb != null)
            {
                return ServiceError.BadRequest("Client already exists");
            }
            
            var client = new Client
            {
                Name = request.Name ?? throw new ArgumentException("Name is required"),
                Email = request.Email ?? throw new ArgumentException("Email is required"),
                Phone = request.Phone ?? throw new ArgumentException("Phone is required"),
                Age = request.Age,
                Gender = Enum.Parse<Gender>(_textInfo.ToTitleCase(request.Gender) ?? throw new ArgumentException("Gender is required")),
            };
            
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            
            return ServiceResult.Success(client.Id);
        }
        catch (Exception ex)
        {
            return ServiceResult.Error(ex.Message);
        }
    }
    
    public async Task<ServiceResult> UpdateClient(UpdateClientDto request, int id)
    {
        try
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
            {
                return ServiceError.NotFound("Client not found");
            }
            
            client.Name = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : client.Name;
            client.Email = !string.IsNullOrWhiteSpace(request.Email) ? request.Email : client.Email;
            client.Phone = !string.IsNullOrWhiteSpace(request.Phone) ? request.Phone : client.Phone;
            client.Age = !string.IsNullOrWhiteSpace(request.Age) ? request.Age : client.Age;
            client.Gender = !string.IsNullOrWhiteSpace(request.Gender) ? Enum.Parse<Gender>(_textInfo.ToTitleCase(request.Gender)) : client.Gender;
            
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            
            return ServiceResult.Success(client.Id);
        }
        catch (Exception ex)
        {
            return ServiceResult.Error(ex.Message);
        }
    }

    public async Task<ServiceResult> DeleteClient(int id)
    {
        try
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
            {
                return ServiceError.NotFound("Client not found");
            }
            
            client.Deleted = true;
            client.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return ServiceResult.Success(client.Id);
        }
        catch (Exception ex)
        {
            return ServiceResult.Error(ex.Message);
        }
    }
}
