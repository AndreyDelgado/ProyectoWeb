namespace Proyecto.Domain.DTO;

public sealed class TechnicianListDto
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string IdentificationNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public bool IsAvailable { get; init; }
    public decimal HourlyRate { get; init; }
    public IReadOnlyList<TechnicianSpecialtyListDto> Specialties { get; init; } =
        Array.Empty<TechnicianSpecialtyListDto>();
}

public sealed class TechnicianSpecialtyListDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CertificationLevel { get; init; } = string.Empty;
    public DateOnly CertifiedSince { get; init; }
}
