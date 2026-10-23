using System.Text.Json.Serialization;

namespace Proyecto.Domain.DTO.ResultadoPatron;

/// <summary>
/// En resultados no paginados, los campos de paginación quedan en null y JsonIgnore los omite:
/// una consulta por identificador no devuelve campos de paginación vacíos.
/// </summary>
public class Resultado<T> : ResultadoBase
{
    [JsonPropertyOrder(1)]
    [JsonPropertyName("data")]
    public T? Datos { get; set; }

    [JsonPropertyOrder(2), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("pageNumber")]
    public int? NumeroPagina { get; set; }

    [JsonPropertyOrder(3), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("pageSize")]
    public int? TamanoPagina { get; set; }

    [JsonPropertyOrder(4), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("totalRecords")]
    public int? RegistrosTotales { get; set; }

    [JsonPropertyOrder(5), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("totalPages")]
    public int? PaginasTotales { get; set; }

    public static Resultado<T> Exito(T datos, int codigoEstado = 200, string titulo = "Correcto", string detalle = "") =>
        new() { EsExitoso = true, Estado = codigoEstado, Titulo = titulo, Detalle = detalle, Datos = datos };

    public static Resultado<T> Fallo(int codigoEstado, string titulo, string detalle) =>
        new() { EsExitoso = false, Estado = codigoEstado, Titulo = titulo, Detalle = detalle };

    public static Resultado<T> Paginado(T datos, int numeroPagina, int tamanoPagina, int registrosTotales) =>
        new()
        {
            EsExitoso = true,
            Estado = 200,
            Titulo = "Correcto",
            Datos = datos,
            NumeroPagina = numeroPagina,
            TamanoPagina = tamanoPagina,
            RegistrosTotales = registrosTotales,
            // Se redondea hacia arriba: 23 registros de 10 en 10 son 3 páginas, no 2.
            PaginasTotales = (int)Math.Ceiling(registrosTotales / (double)tamanoPagina)
        };
}
