namespace Proyecto.Domain.DTO;

public class ActivoDetalleDto
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Identificador { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("assetCode")]
    public string? CodigoActivo { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string? Nombre { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("description")]
    public string? Descripcion { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("brand")]
    public string? Marca { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("model")]
    public string? Modelo { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("serialNumber")]
    public string? NumeroSerie { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("acquiredOn")]
    public DateTime FechaAdquisicion { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("purchaseCost")]
    public decimal CostoCompra { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("usefulLifeMonths")]
    public int MesesVidaUtil { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("status")]
    public string? Estado { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("criticality")]
    public string? Criticidad { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("usageHours")]
    public int HorasUso { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("usageHoursAtLastMaintenance")]
    public int HorasUsoAlUltimoMantenimiento { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("image")]
    public string? Imagen { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("imageUrl")]
    public string? UrlImagen { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("categoryId")]
    public int CategoriaActivoId { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("categoryName")]
    public string? NombreCategoria { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("locationId")]
    public int UbicacionId { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("locationName")]
    public string? NombreUbicacion { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("supplierId")]
    public int? ProveedorId { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("supplierName")]
    public string? NombreProveedor { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("createdAt")]
    public DateTime CreadoEn { get; set; }
}
