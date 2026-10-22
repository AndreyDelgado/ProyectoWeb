using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;

namespace Proyecto.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MaintenanceOrderController : ControllerBase
{
    private readonly IMaintenanceOrderService _service;

    public MaintenanceOrderController(IMaintenanceOrderService service)
    {
        _service = service;
    }

    // GET api/MaintenanceOrder
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();

        return StatusCode(result.Status, result);
    }

    // GET api/MaintenanceOrder/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);

        return StatusCode(result.Status, result);
    }

    // GET api/MaintenanceOrder/{id}
    [HttpGet("{id:int}", Name = "GetMaintenanceOrderById")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return StatusCode(result.Status, result);
    }

    // GET api/MaintenanceOrder/by-asset/{assetId}
    [HttpGet("by-asset/{assetId:int}")]
    public async Task<IActionResult> GetByAssetAsync(int assetId)
    {
        var result = await _service.GetByAssetAsync(assetId);

        return StatusCode(result.Status, result);
    }

    // POST api/MaintenanceOrder
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] MaintenanceOrderCreateDto order)
    {
        var result = await _service.AddAsync(order);

        if (!result.IsSuccess)
            return StatusCode(result.Status, result);

        return CreatedAtRoute("GetMaintenanceOrderById", new { id = result.Data!.Id }, result);
    }
}
