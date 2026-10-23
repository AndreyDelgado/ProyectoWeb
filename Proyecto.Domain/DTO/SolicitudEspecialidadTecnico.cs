using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public sealed record SolicitudEspecialidadTecnico
{
    /// <summary>Nivel de certificación: Basic, Intermediate o Advanced.</summary>
    [Required]
    [RegularExpression("^(Basic|Intermediate|Advanced)$")]
    [System.Text.Json.Serialization.JsonPropertyName("certificationLevel")]
    public required string NivelCertificacion { get; init; }

    /// <summary>Fecha en que se otorgó la certificación.</summary>
    [DataType(DataType.Date)]
    [System.Text.Json.Serialization.JsonPropertyName("certifiedSince")]
    public DateOnly CertificadoDesde { get; init; }
}
