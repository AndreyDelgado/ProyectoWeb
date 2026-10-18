namespace Proyecto.Domain.Entities;

public class AssetCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PreventiveThresholdHours { get; set; }
    public string CodePrefix { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<Asset> Assets { get; set; } = new List<Asset>();
}