using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class ActualizarEstadoActivoDto
{
    // El estado UnderMaintenance depende de las órdenes y Decommissioned de la eliminación.
    [Required(ErrorMessage = "El estado es obligatorio.")]
    [RegularExpression("^(Operational|OutOfService)$", ErrorMessage = "El estado solo puede ser Operational u OutOfService.")]
    [System.Text.Json.Serialization.JsonPropertyName("status")]
    public string? Estado { get; set; }
}
