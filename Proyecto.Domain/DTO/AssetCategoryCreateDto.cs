using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class AssetCategoryCreateDto
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
    public string? Name { get; set; }

    [StringLength(250, ErrorMessage = "La descripción no puede superar 250 caracteres.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "El prefijo de código es obligatorio.")]
    [RegularExpression("^[A-Z]{2,5}$", ErrorMessage = "El prefijo debe tener de 2 a 5 letras mayúsculas.")]
    public string? CodePrefix { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Las horas para mantenimiento preventivo deben ser mayores que 0.")]
    public int PreventiveThresholdHours { get; set; }
}
