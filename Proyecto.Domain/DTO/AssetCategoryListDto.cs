namespace Proyecto.Domain.DTO;

public class AssetCategoryListDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CodePrefix { get; set; }
    public int PreventiveThresholdHours { get; set; }
    public bool IsActive { get; set; }
}
