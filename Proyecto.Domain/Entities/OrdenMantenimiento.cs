namespace Proyecto.Domain.Entities;

public class OrdenMantenimiento
{
    public int Identificador { get; set; }
    public string CodigoOrdenTrabajo { get; set; } = string.Empty;
    public int ActivoId { get; set; }
    public Activo Activo { get; set; } = null!;
    public int TipoMantenimientoId { get; set; }
    public TipoMantenimiento TipoMantenimiento { get; set; } = null!;
    public int? ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Prioridad { get; set; } = "Medium";
    public string Estado { get; set; } = "Requested";
    public DateTime SolicitadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? ProgramadoPara { get; set; }
    public DateTime? IniciadoEn { get; set; }
    public DateTime? CerradoEn { get; set; }
    public decimal? CostoTotal { get; set; }
    public string? Hallazgos { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}