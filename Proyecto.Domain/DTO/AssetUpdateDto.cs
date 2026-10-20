using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class AssetUpdateDto
{
    [Required(ErrorMessage = "El nombre del activo es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 120 caracteres.")]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    public string? Description { get; set; }

    [StringLength(80, ErrorMessage = "La marca no puede superar 80 caracteres.")]
    public string? Brand { get; set; }

    [StringLength(80, ErrorMessage = "El modelo no puede superar 80 caracteres.")]
    public string? Model { get; set; }

    [StringLength(100, ErrorMessage = "El número de serie no puede superar 100 caracteres.")]
    public string? SerialNumber { get; set; }

    [Required(ErrorMessage = "La fecha de adquisición es obligatoria.")]
    public DateTime? AcquiredOn { get; set; }

    [Range(0, 999999999.99, ErrorMessage = "El costo de compra debe estar entre 0 y 999999999.99.")]
    public decimal PurchaseCost { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La vida útil debe ser de al menos 1 mes.")]
    public int UsefulLifeMonths { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Las horas de uso no pueden ser negativas.")]
    public int UsageHours { get; set; }

    [Required(ErrorMessage = "La criticidad es obligatoria.")]
    [RegularExpression("^(High|Medium|Low)$", ErrorMessage = "La criticidad debe ser High, Medium o Low.")]
    public string? Criticality { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La categoría es obligatoria.")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La ubicación es obligatoria.")]
    public int LocationId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El proveedor no es válido.")]
    public int? SupplierId { get; set; }
}
