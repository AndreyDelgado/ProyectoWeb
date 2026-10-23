namespace Proyecto.Domain.DTO;

public class ActivoListaDto
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Identificador { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("assetCode")]
    public string? CodigoActivo { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string? Nombre { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("status")]
    public string? Estado { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("criticality")]
    public string? Criticidad { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("categoryName")]
    public string? NombreCategoria { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("locationName")]
    public string? NombreUbicacion { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("imageUrl")]
    public string? UrlImagen { get; set; }
}
