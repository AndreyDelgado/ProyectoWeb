using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssetController : ControllerBase
{
    private readonly IAssetService _service;

    public AssetController(IAssetService service)
    {
        _service = service;
    }

    // GET api/Asset
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();

        return StatusCode(result.Status, result);
    }

    // GET api/Asset/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);

        return StatusCode(result.Status, result);
    }

    // GET api/Asset/{id}
    [HttpGet("{id:int}", Name = "GetAssetById")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return StatusCode(result.Status, result);
    }

    // POST api/Asset
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] AssetCreateDto asset)
    {
        var result = await _service.AddAsync(asset);

        if (!result.IsSuccess)
            return StatusCode(result.Status, result);

        return CreatedAtRoute("GetAssetById", new { id = result.Data!.Id }, result);
    }

    // PUT api/Asset/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] AssetUpdateDto asset)
    {
        var result = await _service.UpdateAsync(id, asset);

        return StatusCode(result.Status, result);
    }

    // PATCH api/Asset/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatusAsync([FromRoute] int id, [FromBody] AssetStatusUpdateDto status)
    {
        var result = await _service.UpdateStatusAsync(id, status);

        return StatusCode(result.Status, result);
    }

    // DELETE api/Asset/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);

        return StatusCode(result.Status, result);
    }

    // POST api/Asset/{id}/image (multipart/form-data, campo "file")
    [HttpPost("{id:int}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImageAsync(int id,
        [Required(ErrorMessage = "El archivo de imagen es obligatorio.")] IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var result = await _service.UploadImageAsync(id, stream, file.FileName, file.Length);

        return StatusCode(result.Status, result);
    }

    // DELETE api/Asset/{id}/image
    [HttpDelete("{id:int}/image")]
    public async Task<IActionResult> DeleteImageAsync(int id)
    {
        var result = await _service.DeleteImageAsync(id);

        return StatusCode(result.Status, result);
    }
}
