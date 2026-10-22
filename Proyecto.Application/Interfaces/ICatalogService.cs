using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;

namespace Proyecto.Application.Interfaces;

public interface ICatalogService
{
    Task<Result<IEnumerable<LocationListDto>>> GetLocationsAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<MaintenanceTypeListDto>>> GetMaintenanceTypesAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<SpecialtyListDto>>> GetSpecialtiesAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<TechnicianListDto>>> GetTechniciansAsync(CancellationToken cancellationToken);
    Task<Result<TechnicianListDto>> GetTechnicianAsync(int id, CancellationToken cancellationToken);
}
