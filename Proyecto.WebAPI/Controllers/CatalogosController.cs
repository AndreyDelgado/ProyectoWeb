using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;

namespace Proyecto.WebAPI.Controllers;

[Route("api")]
[ApiController]
public sealed class CatalogosController(ICatalogoService catalogService) : ControllerBase
{
    [HttpGet("locations")]
    public async Task<IActionResult> ObtenerUbicacionesAsync(CancellationToken tokenCancelacion)
    {
        var resultado = await catalogService.ObtenerUbicacionesAsync(tokenCancelacion);
        return StatusCode(resultado.Estado, resultado);
    }

    [HttpGet("maintenance-types")]
    public async Task<IActionResult> ObtenerTiposMantenimientoAsync(CancellationToken tokenCancelacion)
    {
        var resultado = await catalogService.ObtenerTiposMantenimientoAsync(tokenCancelacion);
        return StatusCode(resultado.Estado, resultado);
    }
}
