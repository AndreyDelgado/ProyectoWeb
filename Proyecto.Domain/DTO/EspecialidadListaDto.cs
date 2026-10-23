namespace Proyecto.Domain.DTO;

public sealed class EspecialidadListaDto
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Identificador { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Nombre { get; init; } = string.Empty;
    [System.Text.Json.Serialization.JsonPropertyName("description")]
    public string? Descripcion { get; init; }
}
