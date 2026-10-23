namespace Proyecto.Domain.DTO;

public class ProveedorListaDto
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Identificador { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string? Nombre { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("taxId")]
    public string? IdentificacionFiscal { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("contactName")]
    public string? NombreContacto { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("phone")]
    public string? Telefono { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("isActive")]
    public bool EstaActivo { get; set; }
}
