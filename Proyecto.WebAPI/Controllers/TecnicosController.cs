using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/technicians")]
[ApiController]
public sealed class TecnicosController(ICatalogoService catalogService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync(CancellationToken tokenCancelacion)
    {
        var resultado = await catalogService.ObtenerTecnicosAsync(tokenCancelacion);
        return StatusCode(resultado.Estado, resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorIdentificadorAsync(int id, CancellationToken tokenCancelacion)
    {
        var resultado = await catalogService.ObtenerTecnicoAsync(id, tokenCancelacion);
        return StatusCode(resultado.Estado, resultado);
    }

    [HttpPost("{id:int}/specialties/{specialtyId:int}")]
    public async Task<IActionResult> AgregarEspecialidadAsync(
        int id,
        int specialtyId,
        [FromBody] SolicitudEspecialidadTecnico solicitud,
        CancellationToken tokenCancelacion)
    {
        var resultado = await catalogService.AgregarEspecialidadTecnicoAsync(id, specialtyId, solicitud, tokenCancelacion);
        return StatusCode(resultado.Estado, resultado);
    }

    [HttpDelete("{id:int}/specialties/{specialtyId:int}")]
    public async Task<IActionResult> QuitarEspecialidadAsync(
        int id,
        int specialtyId,
        CancellationToken tokenCancelacion)
    {
        var resultado = await catalogService.QuitarEspecialidadTecnicoAsync(id, specialtyId, tokenCancelacion);
        return resultado.EsExitoso
            ? NoContent()
            : StatusCode(resultado.Estado, resultado);
    }
}
