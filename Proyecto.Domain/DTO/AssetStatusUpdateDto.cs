using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class AssetStatusUpdateDto
{
    // UnderMaintenance depende de las órdenes y Decommissioned del DELETE
    [Required(ErrorMessage = "El estado es obligatorio.")]
    [RegularExpression("^(Operational|OutOfService)$", ErrorMessage = "El estado solo puede ser Operational u OutOfService.")]
    public string? Status { get; set; }
}
