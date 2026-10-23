using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/AssetCategory")]
[ApiController]
public class CategoriaActivoController : ControllerBase
{
    private readonly ICategoriaActivoService _servicio;

    public CategoriaActivoController(ICategoriaActivoService servicio)
    {
        _servicio = servicio;
    }

    // OBTENER /api/AssetCategory
    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var resultado = await _servicio.ObtenerTodosAsync();

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/AssetCategory/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> ObtenerPaginacionAsync([FromRoute(Name = "pageNumber")] int paginaNumero = 1, [FromRoute(Name = "pageSize")] int tamanoPagina = 10)
    {
        var resultado = await _servicio.ObtenerPaginacionAsync(paginaNumero, tamanoPagina);

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/AssetCategory/{id}
    [HttpGet("{id:int}", Name = "GetAssetCategoryById")]
    public async Task<IActionResult> ObtenerPorIdentificadorAsync(int id)
    {
        var resultado = await _servicio.ObtenerPorIdentificadorAsync(id);

        return StatusCode(resultado.Estado, resultado);
    }

    // CREAR /api/AssetCategory
    [HttpPost]
    public async Task<IActionResult> AgregarAsync([FromBody] CategoriaActivoCrearDto categoria)
    {
        var resultado = await _servicio.AgregarAsync(categoria);

        if (!resultado.EsExitoso)
            return StatusCode(resultado.Estado, resultado);

        // 201 Created con el encabezado Ubicacion: /api/CategoriaActivo/{id}
        return CreatedAtRoute("GetAssetCategoryById", new { id = resultado.Datos!.Identificador }, resultado);
    }

    // ACTUALIZAR /api/AssetCategory/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarAsync([FromRoute] int id, [FromBody] CategoriaActivoActualizarDto categoria)
    {
        var resultado = await _servicio.ActualizarAsync(id, categoria);

        return StatusCode(resultado.Estado, resultado);
    }

    // ELIMINAR /api/AssetCategory/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarAsync(int id)
    {
        var resultado = await _servicio.EliminarAsync(id);

        return StatusCode(resultado.Estado, resultado);
    }
}
