using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/Asset")]
[ApiController]
public class ActivoController : ControllerBase
{
    private readonly IActivoService _servicio;

    public ActivoController(IActivoService servicio)
    {
        _servicio = servicio;
    }

    // OBTENER /api/Asset
    [HttpGet]
    public async Task<IActionResult> ObtenerTodosAsync()
    {
        var resultado = await _servicio.ObtenerTodosAsync();

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/Asset/{paginaNumero}/{tamanoPagina}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> ObtenerPaginacionAsync([FromRoute(Name = "pageNumber")] int paginaNumero = 1, [FromRoute(Name = "pageSize")] int tamanoPagina = 10)
    {
        var resultado = await _servicio.ObtenerPaginacionAsync(paginaNumero, tamanoPagina);

        return StatusCode(resultado.Estado, resultado);
    }

    // OBTENER /api/Asset/{id}
    [HttpGet("{id:int}", Name = "GetAssetById")]
    public async Task<IActionResult> ObtenerPorIdentificadorAsync(int id)
    {
        var resultado = await _servicio.ObtenerPorIdentificadorAsync(id);

        return StatusCode(resultado.Estado, resultado);
    }

    // CREAR /api/Asset
    [HttpPost]
    public async Task<IActionResult> AgregarAsync([FromBody] ActivoCrearDto activo)
    {
        var resultado = await _servicio.AgregarAsync(activo);

        if (!resultado.EsExitoso)
            return StatusCode(resultado.Estado, resultado);

        return CreatedAtRoute("GetAssetById", new { id = resultado.Datos!.Identificador }, resultado);
    }

    // ACTUALIZAR /api/Asset/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarAsync([FromRoute] int id, [FromBody] ActivoActualizarDto activo)
    {
        var resultado = await _servicio.ActualizarAsync(id, activo);

        return StatusCode(resultado.Estado, resultado);
    }

    // ACTUALIZAR PARCIALMENTE /api/Asset/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ActualizarEstadoAsync([FromRoute] int id, [FromBody] ActualizarEstadoActivoDto estado)
    {
        var resultado = await _servicio.ActualizarEstadoAsync(id, estado);

        return StatusCode(resultado.Estado, resultado);
    }

    // ELIMINAR /api/Asset/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarAsync(int id)
    {
        var resultado = await _servicio.EliminarAsync(id);

        return StatusCode(resultado.Estado, resultado);
    }

    // CREAR /api/Asset/{id}/imagen (multipart/form-datos, campo "file")
    [HttpPost("{id:int}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SubirImagenAsync(int id,
        [FromForm(Name = "file")] [Required(ErrorMessage = "El archivo de imagen es obligatorio.")] IFormFile archivo)
    {
        await using var stream = archivo.OpenReadStream();

        var resultado = await _servicio.SubirImagenAsync(id, stream, archivo.FileName, archivo.Length);

        return StatusCode(resultado.Estado, resultado);
    }

    // ELIMINAR /api/Asset/{id}/imagen
    [HttpDelete("{id:int}/image")]
    public async Task<IActionResult> EliminarImagenAsync(int id)
    {
        var resultado = await _servicio.EliminarImagenAsync(id);

        return StatusCode(resultado.Estado, resultado);
    }
}
