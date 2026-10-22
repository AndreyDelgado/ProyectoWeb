using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;

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
}
