using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssetCategoryController : ControllerBase
{
    private readonly IAssetCategoryService _service;

    public AssetCategoryController(IAssetCategoryService service)
    {
        _service = service;
    }

    // GET api/AssetCategory
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();

        return StatusCode(result.Status, result);
    }

    // GET api/AssetCategory/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);

        return StatusCode(result.Status, result);
    }

    // GET api/AssetCategory/{id}
    [HttpGet("{id:int}", Name = "GetAssetCategoryById")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return StatusCode(result.Status, result);
    }

    // POST api/AssetCategory
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] AssetCategoryCreateDto category)
    {
        var result = await _service.AddAsync(category);

        if (!result.IsSuccess)
            return StatusCode(result.Status, result);

        // 201 Created con el encabezado Location: /api/AssetCategory/{id}
        return CreatedAtRoute("GetAssetCategoryById", new { id = result.Data!.Id }, result);
    }

    // PUT api/AssetCategory/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] AssetCategoryUpdateDto category)
    {
        var result = await _service.UpdateAsync(id, category);

        return StatusCode(result.Status, result);
    }

    // DELETE api/AssetCategory/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);

        return StatusCode(result.Status, result);
    }
}
