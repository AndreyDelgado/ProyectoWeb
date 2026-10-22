using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/technicians")]
[ApiController]
public sealed class TechniciansController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await catalogService.GetTechniciansAsync(cancellationToken);
        return StatusCode(result.Status, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await catalogService.GetTechnicianAsync(id, cancellationToken);
        return StatusCode(result.Status, result);
    }

    [HttpPost("{id:int}/specialties/{specialtyId:int}")]
    public async Task<IActionResult> AddSpecialtyAsync(
        int id,
        int specialtyId,
        [FromBody] TechnicianSpecialtyRequest request,
        CancellationToken cancellationToken)
    {
        var result = await catalogService.AddTechnicianSpecialtyAsync(id, specialtyId, request, cancellationToken);
        return StatusCode(result.Status, result);
    }

    [HttpDelete("{id:int}/specialties/{specialtyId:int}")]
    public async Task<IActionResult> RemoveSpecialtyAsync(
        int id,
        int specialtyId,
        CancellationToken cancellationToken)
    {
        var result = await catalogService.RemoveTechnicianSpecialtyAsync(id, specialtyId, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : StatusCode(result.Status, result);
    }
}
