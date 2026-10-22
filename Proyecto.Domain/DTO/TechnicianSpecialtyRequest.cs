using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public sealed record TechnicianSpecialtyRequest
{
    /// <summary>Certification level: Basic, Intermediate, or Advanced.</summary>
    [Required]
    [RegularExpression("^(Basic|Intermediate|Advanced)$")]
    public required string CertificationLevel { get; init; }

    /// <summary>Date when the certification was granted.</summary>
    [DataType(DataType.Date)]
    public DateOnly CertifiedSince { get; init; }
}
