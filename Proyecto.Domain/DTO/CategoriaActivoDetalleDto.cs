namespace Proyecto.Domain.DTO;

public class CategoriaActivoDetalleDto
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Identificador { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string? Nombre { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("description")]
    public string? Descripcion { get; set; }
    public string? PrefijoCodigo { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("preventiveThresholdHours")]
    public int UmbralPreventivoHoras { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("isActive")]
    public bool EstaActivo { get; set; }
}
