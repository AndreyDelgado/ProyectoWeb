using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Interfaces;

public interface IActivoRepository
{
    // Lectura
    Task<IEnumerable<Activo>> ObtenerTodosAsync();
    Task<IEnumerable<Activo>> ObtenerPaginacionAsync(int paginaNumero, int tamanoPagina);
    Task<int> ContarAsync();
    Task<Activo?> ObtenerPorIdentificadorAsync(int id);
    Task<Activo?> ObtenerParaActualizarAsync(int id);

    // Verificaciones para las reglas de negocio
    Task<string?> ObtenerPrefijoCodigoCategoriaAsync(int categoryId);
    Task<bool> ExisteUbicacionAsync(int locationId);
    Task<bool> ExisteProveedorAsync(int supplierId);
    Task<bool> ExistePorNumeroSerieAsync(string serialNumber, int? excludeId = null);
    Task<bool> TieneOrdenesAbiertasAsync(int id, IEnumerable<string> closedStatuses);
    Task<string?> ObtenerUltimoCodigoActivoAsync(string codePrefix);

    // Escritura
    Task AgregarAsync(Activo activo);
    Task GuardarCambiosAsync();
}
