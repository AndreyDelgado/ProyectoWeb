namespace Proyecto.Domain.DTO;

public class AssetListDto
{
    public int Id { get; set; }
    public string? AssetCode { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? Criticality { get; set; }
    public string? CategoryName { get; set; }
    public string? LocationName { get; set; }
    public string? ImageUrl { get; set; }
}
