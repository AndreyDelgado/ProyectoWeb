namespace Proyecto.Domain.Entities;

public class Ubicacion
{
    public int Identificador { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool EstaActivo { get; set; } = true;
    public ICollection<Activo> Activos { get; set; } = new List<Activo>();
}