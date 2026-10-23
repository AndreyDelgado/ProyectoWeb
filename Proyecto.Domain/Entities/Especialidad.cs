namespace Proyecto.Domain.Entities;

public class Especialidad
{
    public int Identificador { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public ICollection<TecnicoEspecialidad> EspecialidadesTecnico { get; set; } = new List<TecnicoEspecialidad>();
    public ICollection<TipoMantenimiento> TiposMantenimiento { get; set; } = new List<TipoMantenimiento>();
}