using System.ComponentModel.DataAnnotations;

namespace Proyecto.Domain.DTO;

public class SupplierUpdateDto
{
    [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 120 caracteres.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "La cédula jurídica es obligatoria.")]
    [RegularExpression(@"^\d-\d{3}-\d{6}$", ErrorMessage = "La cédula jurídica debe tener el formato 3-101-123456.")]
    public string? TaxId { get; set; }

    [StringLength(120, ErrorMessage = "El nombre del contacto no puede superar 120 caracteres.")]
    public string? ContactName { get; set; }

    [RegularExpression(@"^\d{4}-?\d{4}$", ErrorMessage = "El teléfono debe tener 8 dígitos, con o sin guion: 2201-1001.")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    [StringLength(254, ErrorMessage = "El correo no puede superar 254 caracteres.")]
    public string? Email { get; set; }

    [StringLength(250, ErrorMessage = "La dirección no puede superar 250 caracteres.")]
    public string? Address { get; set; }

    [StringLength(500, ErrorMessage = "Las condiciones generales no pueden superar 500 caracteres.")]
    public string? GeneralConditions { get; set; }

    public bool IsActive { get; set; }
}
