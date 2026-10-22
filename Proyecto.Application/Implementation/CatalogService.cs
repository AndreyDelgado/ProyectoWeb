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

    public async Task<Result<IEnumerable<SpecialtyListDto>>> GetSpecialtiesAsync(
        CancellationToken cancellationToken)
    {
        var specialties = await context.Specialties
            .AsNoTracking()
            .OrderBy(specialty => specialty.Name)
            .Select(specialty => new SpecialtyListDto
            {
                Id = specialty.Id,
                Name = specialty.Name,
                Description = specialty.Description
            })
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<SpecialtyListDto>>.Success(specialties);
    }

    public async Task<Result<IEnumerable<TechnicianListDto>>> GetTechniciansAsync(
        CancellationToken cancellationToken)
    {
        var technicians = await context.Technicians
            .AsNoTracking()
            .Include(technician => technician.TechnicianSpecialties)
                .ThenInclude(link => link.Specialty)
            .OrderBy(technician => technician.FullName)
            .ToListAsync(cancellationToken);

        var data = technicians.Select(technician => new TechnicianListDto
        {
            Id = technician.Id,
            FullName = technician.FullName,
            IdentificationNumber = technician.IdentificationNumber,
            Email = technician.Email,
            Phone = technician.Phone,
            IsAvailable = technician.IsAvailable,
            HourlyRate = technician.HourlyRate,
            Specialties = technician.TechnicianSpecialties
                .OrderBy(link => link.Specialty.Name)
                .Select(link => new TechnicianSpecialtyListDto
                {
                    Id = link.SpecialtyId,
                    Name = link.Specialty.Name,
                    CertificationLevel = link.CertificationLevel,
                    CertifiedSince = DateOnly.FromDateTime(link.CertifiedSince)
                })
                .ToArray()
        }).ToArray();

        return Result<IEnumerable<TechnicianListDto>>.Success(data);
    }
}
