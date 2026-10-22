using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;

namespace Proyecto.WebAPI.Controllers;

[Route("api")]
[ApiController]
public sealed class CatalogsController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet("locations")]
    public async Task<IActionResult> GetLocationsAsync(CancellationToken cancellationToken)
    {
        var result = await catalogService.GetLocationsAsync(cancellationToken);
        return StatusCode(result.Status, result);
    }

    [HttpGet("maintenance-types")]
    public async Task<IActionResult> GetMaintenanceTypesAsync(CancellationToken cancellationToken)
    {
        var result = await catalogService.GetMaintenanceTypesAsync(cancellationToken);
        return StatusCode(result.Status, result);
    }
}
