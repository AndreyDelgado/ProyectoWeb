using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;

namespace Proyecto.WebAPI.Controllers;

[Route("api/specialties")]
[ApiController]
public sealed class SpecialtiesController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await catalogService.GetSpecialtiesAsync(cancellationToken);
        return StatusCode(result.Status, result);
    }
}
