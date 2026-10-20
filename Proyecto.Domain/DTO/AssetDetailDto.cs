namespace Proyecto.Domain.DTO;

public class AssetDetailDto
{
    public int Id { get; set; }
    public string? AssetCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime AcquiredOn { get; set; }
    public decimal PurchaseCost { get; set; }
    public int UsefulLifeMonths { get; set; }
    public string? Status { get; set; }
    public string? Criticality { get; set; }
    public int UsageHours { get; set; }
    public int UsageHoursAtLastMaintenance { get; set; }
    public string? Image { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int LocationId { get; set; }
    public string? LocationName { get; set; }
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public DateTime CreatedAt { get; set; }
}
