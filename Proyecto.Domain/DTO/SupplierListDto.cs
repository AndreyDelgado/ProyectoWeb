namespace Proyecto.Domain.DTO;

public class SupplierListDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? TaxId { get; set; }
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
}
