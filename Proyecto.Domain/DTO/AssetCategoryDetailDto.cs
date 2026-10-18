namespace Proyecto.Domain.DTO;

public class AssetCategoryDetailDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CodePrefix { get; set; }
    public int PreventiveThresholdHours { get; set; }
    public bool IsActive { get; set; }
}
