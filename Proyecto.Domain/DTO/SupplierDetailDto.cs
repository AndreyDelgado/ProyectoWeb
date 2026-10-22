namespace Proyecto.Domain.DTO;

public class SupplierDetailDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? TaxId { get; set; }
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? GeneralConditions { get; set; }
    public bool IsActive { get; set; }
}
