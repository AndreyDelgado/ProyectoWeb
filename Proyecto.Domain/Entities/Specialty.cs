namespace Proyecto.Domain.Entities;

public class Specialty
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<TechnicianSpecialty> TechnicianSpecialties { get; set; } = new List<TechnicianSpecialty>();
    public ICollection<MaintenanceType> MaintenanceTypes { get; set; } = new List<MaintenanceType>();
}