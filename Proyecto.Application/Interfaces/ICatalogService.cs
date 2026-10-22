using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;

namespace Proyecto.Application.Interfaces;

public interface ICatalogService
{
    Task<Result<IEnumerable<LocationListDto>>> GetLocationsAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<MaintenanceTypeListDto>>> GetMaintenanceTypesAsync(CancellationToken cancellationToken);
}
