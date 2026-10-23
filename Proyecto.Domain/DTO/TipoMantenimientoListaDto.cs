namespace Proyecto.Domain.DTO;

public sealed class TipoMantenimientoListaDto
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Identificador { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Nombre { get; init; } = string.Empty;
    [System.Text.Json.Serialization.JsonPropertyName("description")]
    public string? Descripcion { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("defaultIntervalDays")]
    public int? IntervaloPredeterminadoDias { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("isEmergency")]
    public bool EsEmergencia { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("requiredSpecialtyId")]
    public int? EspecialidadRequeridaId { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("requiredSpecialtyName")]
    public string? NombreEspecialidadRequerida { get; init; }
}
