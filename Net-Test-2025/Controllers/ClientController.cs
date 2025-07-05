using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net_Test_2025.Domains;
using Net_Test_2025.Helpers;
using Net_Test_2025.Services.Contracts.DTOs;
using Net_Test_2025.Services.Contracts.Interfaces;

namespace Net_Test_2025.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllClients([FromQuery] ClientsListingRequest request)
    {
        var result = await _clientService.GetAllClients(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return Ok(new {results = result.Data, total = ((List<GetClientDto>)result.Data!).Count});
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        var result = await _clientService.GetClientById(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return Ok(result.Data);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateClient(CreateClientRequest client)
    {
        var result = await _clientService.CreateClient(client);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        
        return CreatedAtAction(nameof(CreateClient), new { id = result.Data }, new { id = result.Data, createdAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, UpdateClientDto client)
    {
        var result = await _clientService.UpdateClient(client, id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        
        return Ok(new { id = result.Data, updatedAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")});
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var result = await _clientService.DeleteClient(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return Ok(new { id = result.Data, deletedAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")});
    }
}
