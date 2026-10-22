using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class MaintenanceOrderCreateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El activo es obligatorio.")]
    public int AssetId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El tipo de mantenimiento es obligatorio.")]
    public int MaintenanceTypeId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El proveedor no es válido.")]
    public int? SupplierId { get; set; }

    [Required(ErrorMessage = "El título de la orden es obligatorio.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 150 caracteres.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "El motivo de la solicitud es obligatorio.")]
    [StringLength(500, MinimumLength = 3, ErrorMessage = "El motivo debe tener entre 3 y 500 caracteres.")]
    public string? Reason { get; set; }

    [StringLength(1000, ErrorMessage = "La descripción no puede superar 1000 caracteres.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "La prioridad es obligatoria.")]
    [RegularExpression("^(Low|Medium|High|Urgent)$", ErrorMessage = "La prioridad debe ser Low, Medium, High o Urgent.")]
    public string? Priority { get; set; }
}
