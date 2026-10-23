namespace Proyecto.Domain.Entities;

public class TipoMantenimiento
{
    public int Identificador { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? IntervaloPredeterminadoDias { get; set; }
    public bool EsEmergencia { get; set; }
    public int? EspecialidadRequeridaId { get; set; }
    public Especialidad? EspecialidadRequerida { get; set; }
    public ICollection<OrdenMantenimiento> OrdenesMantenimiento { get; set; } = new List<OrdenMantenimiento>();
}