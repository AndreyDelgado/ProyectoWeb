using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class CategoriaActivoCrearDto
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string? Nombre { get; set; }

    [StringLength(250, ErrorMessage = "La descripción no puede superar 250 caracteres.")]
    [System.Text.Json.Serialization.JsonPropertyName("description")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El prefijo de código es obligatorio.")]
    [RegularExpression("^[A-Z]{2,5}$", ErrorMessage = "El prefijo debe tener de 2 a 5 letras mayúsculas.")]
    [System.Text.Json.Serialization.JsonPropertyName("codePrefix")]
    public string? PrefijoCodigo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Las horas para mantenimiento preventivo deben ser mayores que 0.")]
    [System.Text.Json.Serialization.JsonPropertyName("preventiveThresholdHours")]
    public int UmbralPreventivoHoras { get; set; }
}
