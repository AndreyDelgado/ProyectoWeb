namespace Proyecto.Domain.DTO;

public class MaintenanceOrderDetailDto
{
    public int Id { get; set; }
    public string? WorkOrderCode { get; set; }
    public int AssetId { get; set; }
    public string? AssetCode { get; set; }
    public string? AssetName { get; set; }
    public int MaintenanceTypeId { get; set; }
    public string? MaintenanceTypeName { get; set; }
    public bool IsEmergency { get; set; }
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Reason { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ScheduledFor { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public decimal? TotalCost { get; set; }
    public string? Findings { get; set; }
    public DateTime CreatedAt { get; set; }
}
