using Microsoft.EntityFrameworkCore;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;
using Proyecto.Infraestructure.Data;

namespace Proyecto.Application.Implementation;

public sealed class CatalogService(AppDbContext context) : ICatalogService
{
    public async Task<Result<IEnumerable<LocationListDto>>> GetLocationsAsync(
        CancellationToken cancellationToken)
    {
        var locations = await context.Locations
            .AsNoTracking()
            .Where(location => location.IsActive)
            .OrderBy(location => location.Name)
            .Select(location => new LocationListDto
            {
                Id = location.Id,
                Name = location.Name,
                Description = location.Description
            })
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<LocationListDto>>.Success(locations);
    }

    public async Task<Result<IEnumerable<MaintenanceTypeListDto>>> GetMaintenanceTypesAsync(
        CancellationToken cancellationToken)
    {
        var maintenanceTypes = await context.MaintenanceTypes
            .AsNoTracking()
            .OrderBy(type => type.Name)
            .Select(type => new MaintenanceTypeListDto
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description,
                DefaultIntervalDays = type.DefaultIntervalDays,
                IsEmergency = type.IsEmergency,
                RequiredSpecialtyId = type.RequiredSpecialtyId,
                RequiredSpecialtyName = type.RequiredSpecialty == null ? null : type.RequiredSpecialty.Name
            })
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<MaintenanceTypeListDto>>.Success(maintenanceTypes);
    }
}
