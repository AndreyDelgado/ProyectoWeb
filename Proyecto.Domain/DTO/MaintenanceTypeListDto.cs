namespace Proyecto.Domain.DTO;

public sealed class MaintenanceTypeListDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int? DefaultIntervalDays { get; init; }
    public bool IsEmergency { get; init; }
    public int? RequiredSpecialtyId { get; init; }
    public string? RequiredSpecialtyName { get; init; }
}
