using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;

namespace Proyecto.Application.Interfaces;

public interface IMaintenanceOrderService
{
    Task<Result<IEnumerable<MaintenanceOrderListDto>>> GetAllAsync();
    Task<Result<IEnumerable<MaintenanceOrderListDto>>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<Result<MaintenanceOrderDetailDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<MaintenanceOrderListDto>>> GetByAssetAsync(int assetId);
    Task<Result<MaintenanceOrderDetailDto>> AddAsync(MaintenanceOrderCreateDto order);
}
