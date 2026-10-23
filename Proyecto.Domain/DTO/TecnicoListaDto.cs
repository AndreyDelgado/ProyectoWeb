namespace Proyecto.Domain.DTO;

public sealed class TecnicoListaDto
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Identificador { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("fullName")]
    public string NombreCompleto { get; init; } = string.Empty;
    [System.Text.Json.Serialization.JsonPropertyName("identificationNumber")]
    public string NumeroIdentificacion { get; init; } = string.Empty;
    [System.Text.Json.Serialization.JsonPropertyName("email")]
    public string Correo { get; init; } = string.Empty;
    [System.Text.Json.Serialization.JsonPropertyName("phone")]
    public string Telefono { get; init; } = string.Empty;
    [System.Text.Json.Serialization.JsonPropertyName("isAvailable")]
    public bool EstaDisponible { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("hourlyRate")]
    public decimal TarifaPorHora { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("specialties")]
    public IReadOnlyList<TecnicoEspecialidadListaDto> Especialidades { get; init; } =
        Array.Empty<TecnicoEspecialidadListaDto>();
}

public sealed class TecnicoEspecialidadListaDto
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Identificador { get; init; }
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Nombre { get; init; } = string.Empty;
    [System.Text.Json.Serialization.JsonPropertyName("certificationLevel")]
    public string NivelCertificacion { get; init; } = string.Empty;
    [System.Text.Json.Serialization.JsonPropertyName("certifiedSince")]
    public DateOnly CertificadoDesde { get; init; }
}
