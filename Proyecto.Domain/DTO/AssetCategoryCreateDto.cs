using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class AssetCategoryCreateDto
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
    public string? Name { get; set; }

    [StringLength(250, ErrorMessage = "La descripción no puede superar 250 caracteres.")]
    public string? Description { get; set; }
}
