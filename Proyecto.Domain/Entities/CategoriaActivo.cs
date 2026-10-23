namespace Proyecto.Domain.Entities;

public class CategoriaActivo
{
    public int Identificador { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int UmbralPreventivoHoras { get; set; }
    public string PrefijoCodigo { get; set; } = string.Empty;
    public bool EstaActivo { get; set; } = true;
    public ICollection<Activo> Activos { get; set; } = new List<Activo>();
}