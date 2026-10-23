namespace Proyecto.Domain.Entities;

public class TecnicoEspecialidad
{
    public int TecnicoId { get; set; }
    public Tecnico Tecnico { get; set; } = null!;
    public int EspecialidadId { get; set; }
    public Especialidad Especialidad { get; set; } = null!;
    public string NivelCertificacion { get; set; } = string.Empty;
    public DateTime CertificadoDesde { get; set; }
}