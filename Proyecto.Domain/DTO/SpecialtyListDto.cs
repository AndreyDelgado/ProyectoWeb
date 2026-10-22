namespace Proyecto.Domain.DTO;

public sealed class SpecialtyListDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
