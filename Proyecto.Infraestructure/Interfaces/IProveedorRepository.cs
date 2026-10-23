using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Interfaces;

public interface IProveedorRepository
{
    // Lectura
    Task<IEnumerable<Proveedor>> ObtenerTodosAsync();
    Task<IEnumerable<Proveedor>> ObtenerPaginacionAsync(int paginaNumero, int tamanoPagina);
    Task<int> ContarAsync();
    Task<Proveedor?> ObtenerPorIdentificadorAsync(int id);

    // Verificaciones para las reglas de negocio
    Task<bool> ExisteAsync(int id);
    Task<bool> ExistePorIdentificacionFiscalAsync(string taxId, int? excludeId = null);
    Task<bool> TieneRegistrosRelacionadosAsync(int id);

    // Escritura
    Task AgregarAsync(Proveedor proveedor);
    Task ActualizarAsync(Proveedor proveedor);
    Task EliminarAsync(int id);
}
