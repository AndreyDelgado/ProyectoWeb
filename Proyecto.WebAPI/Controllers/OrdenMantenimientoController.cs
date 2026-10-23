using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/MaintenanceOrder")]
[ApiController]
public class OrdenMantenimientoController : ControllerBase
{
    private readonly IOrdenMantenimientoService _servicio;

    public OrdenMantenimientoController(IOrdenMantenimientoService servicio)
    {
        _servicio = servicio;
    }

    // OBTENER /api/MaintenanceOrder
    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var resultado = await _servicio.ObtenerTodosAsync();

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/MaintenanceOrder/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> ObtenerPaginacionAsync([FromRoute(Name = "pageNumber")] int paginaNumero = 1, [FromRoute(Name = "pageSize")] int tamanoPagina = 10)
    {
        var resultado = await _servicio.ObtenerPaginacionAsync(paginaNumero, tamanoPagina);

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/MaintenanceOrder/{id}
    [HttpGet("{id:int}", Name = "GetMaintenanceOrderById")]
    public async Task<IActionResult> ObtenerPorIdentificadorAsync(int id)
    {
        var resultado = await _servicio.ObtenerPorIdentificadorAsync(id);

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/MaintenanceOrder/by-asset/{assetId}
    [HttpGet("by-asset/{assetId:int}")]
    public async Task<IActionResult> ObtenerPorActivoAsync(int assetId)
    {
        var resultado = await _servicio.ObtenerPorActivoAsync(assetId);

        return StatusCode(resultado.Estado, resultado);
    }

    // CREAR /api/MaintenanceOrder
    [HttpPost]
    public async Task<IActionResult> AgregarAsync([FromBody] OrdenMantenimientoCrearDto orden)
    {
        var resultado = await _servicio.AgregarAsync(orden);

        if (!resultado.EsExitoso)
            return StatusCode(resultado.Estado, resultado);

        return CreatedAtRoute("GetMaintenanceOrderById", new { id = resultado.Datos!.Identificador }, resultado);
    }
}
