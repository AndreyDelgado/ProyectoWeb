namespace Proyecto.Domain.DTO;

public class MaintenanceOrderListDto
{
    public int Id { get; set; }
    public string? WorkOrderCode { get; set; }
    public string? AssetCode { get; set; }
    public string? AssetName { get; set; }
    public string? MaintenanceTypeName { get; set; }
    public string? SupplierName { get; set; }
    public string? Title { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public DateTime RequestedAt { get; set; }
}
