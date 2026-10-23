using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;

namespace Proyecto.WebAPI.Controllers;

[Route("api/specialties")]
[ApiController]
public sealed class EspecialidadesController(ICatalogoService catalogService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync(CancellationToken tokenCancelacion)
    {
        var resultado = await catalogService.ObtenerEspecialidadesAsync(tokenCancelacion);
        return StatusCode(resultado.Estado, resultado);
    }
}
