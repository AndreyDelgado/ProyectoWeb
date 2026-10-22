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

        var data = technicians.Select(MapTechnician).ToArray();

        return Result<IEnumerable<TechnicianListDto>>.Success(data);
    }

    public async Task<Result<TechnicianListDto>> GetTechnicianAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var technician = await context.Technicians
            .AsNoTracking()
            .Include(item => item.TechnicianSpecialties)
                .ThenInclude(link => link.Specialty)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        return technician is null
            ? Result<TechnicianListDto>.Failure(404, "Not Found", $"No existe el técnico con id {id}.")
            : Result<TechnicianListDto>.Success(MapTechnician(technician));
    }

    public async Task<Result<TechnicianListDto>> AddTechnicianSpecialtyAsync(
        int technicianId,
        int specialtyId,
        TechnicianSpecialtyRequest request,
        CancellationToken cancellationToken)
    {
        if (request.CertifiedSince == default || request.CertifiedSince > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return Result<TechnicianListDto>.Failure(
                400, "Bad Request", "La fecha de certificación debe ser válida y no puede estar en el futuro.");
        }

        var technician = await context.Technicians
            .FirstOrDefaultAsync(item => item.Id == technicianId, cancellationToken);
        if (technician is null)
        {
            return Result<TechnicianListDto>.Failure(404, "Not Found", $"No existe el técnico con id {technicianId}.");
        }

        var specialtyExists = await context.Specialties
            .AnyAsync(item => item.Id == specialtyId, cancellationToken);
        if (!specialtyExists)
        {
            return Result<TechnicianListDto>.Failure(404, "Not Found", $"No existe la especialidad con id {specialtyId}.");
        }

        var alreadyAssigned = await context.TechnicianSpecialties
            .AnyAsync(link => link.TechnicianId == technicianId && link.SpecialtyId == specialtyId, cancellationToken);
        if (alreadyAssigned)
        {
            return Result<TechnicianListDto>.Failure(
                409, "Conflict", "El técnico ya tiene asignada esta especialidad.");
        }

        context.TechnicianSpecialties.Add(new Domain.Entities.TechnicianSpecialty
        {
            TechnicianId = technicianId,
            SpecialtyId = specialtyId,
            CertificationLevel = request.CertificationLevel,
            CertifiedSince = request.CertifiedSince.ToDateTime(TimeOnly.MinValue)
        });
        await context.SaveChangesAsync(cancellationToken);

        var result = await GetTechnicianAsync(technicianId, cancellationToken);
        if (result.IsSuccess)
        {
            result.Status = 201;
            result.Title = "Created";
        }

        return result;
    }

    public async Task<Result<TechnicianListDto>> RemoveTechnicianSpecialtyAsync(
        int technicianId,
        int specialtyId,
        CancellationToken cancellationToken)
    {
        var technicianExists = await context.Technicians
            .AnyAsync(item => item.Id == technicianId, cancellationToken);
        if (!technicianExists)
        {
            return Result<TechnicianListDto>.Failure(404, "Not Found", $"No existe el técnico con id {technicianId}.");
        }

        var link = await context.TechnicianSpecialties
            .FirstOrDefaultAsync(
                item => item.TechnicianId == technicianId && item.SpecialtyId == specialtyId,
                cancellationToken);
        if (link is null)
        {
            return Result<TechnicianListDto>.Failure(
                404, "Not Found", "El técnico no tiene asignada esta especialidad.");
        }

        context.TechnicianSpecialties.Remove(link);
        await context.SaveChangesAsync(cancellationToken);

        return await GetTechnicianAsync(technicianId, cancellationToken);
    }

    private static TechnicianListDto MapTechnician(Domain.Entities.Technician technician) =>
        new()
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
        };
}
