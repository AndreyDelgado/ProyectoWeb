namespace Proyecto.Domain.Entities;

public class Activo
{
    public int Identificador { get; set; }
    public string CodigoActivo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? NumeroSerie { get; set; }
    public DateTime FechaAdquisicion { get; set; }
    public decimal CostoCompra { get; set; }
    public int MesesVidaUtil { get; set; }
    public string Estado { get; set; } = "Operational";
    public string? Imagen { get; set; }
    public int HorasUso { get; set; }
    public int HorasUsoAlUltimoMantenimiento { get; set; }
    public string Criticidad { get; set; } = "Medium";
    public int CategoriaActivoId { get; set; }
    public CategoriaActivo Categoria { get; set; } = null!;
    public int UbicacionId { get; set; }
    public Ubicacion Ubicacion { get; set; } = null!;
    public int? ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public ICollection<OrdenMantenimiento> OrdenesMantenimiento { get; set; } = new List<OrdenMantenimiento>();
}