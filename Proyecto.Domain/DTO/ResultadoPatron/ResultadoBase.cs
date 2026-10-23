namespace Proyecto.Domain.DTO.ResultadoPatron;

/// <summary>
/// Estado repite el código HTTP dentro del cuerpo, para que el frontend lo lea del JSON
/// sin tener que revisar la respuesta HTTP por aparte.
/// </summary>
public abstract class ResultadoBase
{
    [System.Text.Json.Serialization.JsonPropertyName("isSuccess")]
    public bool EsExitoso { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("statusCode")]
    public int Estado { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("title")]
    public string Titulo { get; set; } = "";
    [System.Text.Json.Serialization.JsonPropertyName("detail")]
    public string Detalle { get; set; } = "";
}
