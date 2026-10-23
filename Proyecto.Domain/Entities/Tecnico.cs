namespace Proyecto.Domain.Entities;

public class Tecnico
{
    public int Identificador { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool EstaDisponible { get; set; } = true;
    public decimal TarifaPorHora { get; set; }
    public ICollection<TecnicoEspecialidad> EspecialidadesTecnico { get; set; } = new List<TecnicoEspecialidad>();
}