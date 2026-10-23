using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/Supplier")]
[ApiController]
public class ProveedorController : ControllerBase
{
    private readonly IProveedorService _servicio;

    public ProveedorController(IProveedorService servicio)
    {
        _servicio = servicio;
    }

    // OBTENER /api/Supplier
    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var resultado = await _servicio.ObtenerTodosAsync();

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/Supplier/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> ObtenerPaginacionAsync([FromRoute(Name = "pageNumber")] int paginaNumero = 1, [FromRoute(Name = "pageSize")] int tamanoPagina = 10)
    {
        var resultado = await _servicio.ObtenerPaginacionAsync(paginaNumero, tamanoPagina);

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/Supplier/{id}
    [HttpGet("{id:int}", Name = "GetSupplierById")]
    public async Task<IActionResult> ObtenerPorIdentificadorAsync(int id)
    {
        var resultado = await _servicio.ObtenerPorIdentificadorAsync(id);

        return StatusCode(resultado.Estado, resultado);
    }

    // CREAR /api/Supplier
    [HttpPost]
    public async Task<IActionResult> AgregarAsync([FromBody] ProveedorCrearDto proveedor)
    {
        var resultado = await _servicio.AgregarAsync(proveedor);

        if (!resultado.EsExitoso)
            return StatusCode(resultado.Estado, resultado);

        return CreatedAtRoute("GetSupplierById", new { id = resultado.Datos!.Identificador }, resultado);
    }

    // ACTUALIZAR /api/Supplier/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarAsync([FromRoute] int id, [FromBody] ProveedorActualizarDto proveedor)
    {
        var resultado = await _servicio.ActualizarAsync(id, proveedor);

        return StatusCode(resultado.Estado, resultado);
    }

    // ELIMINAR /api/Supplier/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarAsync(int id)
    {
        var resultado = await _servicio.EliminarAsync(id);

        return StatusCode(resultado.Estado, resultado);
    }
}
