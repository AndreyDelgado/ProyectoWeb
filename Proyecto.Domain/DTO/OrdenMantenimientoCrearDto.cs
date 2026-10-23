using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class OrdenMantenimientoCrearDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El activo es obligatorio.")]
    [System.Text.Json.Serialization.JsonPropertyName("assetId")]
    public int ActivoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El tipo de mantenimiento es obligatorio.")]
    [System.Text.Json.Serialization.JsonPropertyName("maintenanceTypeId")]
    public int TipoMantenimientoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El proveedor no es válido.")]
    [System.Text.Json.Serialization.JsonPropertyName("supplierId")]
    public int? ProveedorId { get; set; }

    [Required(ErrorMessage = "El título de la orden es obligatorio.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 150 caracteres.")]
    [System.Text.Json.Serialization.JsonPropertyName("title")]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = "El motivo de la solicitud es obligatorio.")]
    [StringLength(500, MinimumLength = 3, ErrorMessage = "El motivo debe tener entre 3 y 500 caracteres.")]
    [System.Text.Json.Serialization.JsonPropertyName("reason")]
    public string? Motivo { get; set; }

    [StringLength(1000, ErrorMessage = "La descripción no puede superar 1000 caracteres.")]
    [System.Text.Json.Serialization.JsonPropertyName("description")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "La prioridad es obligatoria.")]
    [RegularExpression("^(Low|Medium|High|Urgent)$", ErrorMessage = "La prioridad debe ser Low, Medium, High o Urgent.")]
    [System.Text.Json.Serialization.JsonPropertyName("priority")]
    public string? Prioridad { get; set; }
}
