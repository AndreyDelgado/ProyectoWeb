namespace Proyecto.Domain.Entities;

public class Technician
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public decimal HourlyRate { get; set; }
    public ICollection<TechnicianSpecialty> TechnicianSpecialties { get; set; } = new List<TechnicianSpecialty>();
}