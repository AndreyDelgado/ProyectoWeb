using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Interfaces;

public interface IOrdenMantenimientoRepository
{
    // Lectura
    Task<IEnumerable<OrdenMantenimiento>> ObtenerTodosAsync();
    Task<IEnumerable<OrdenMantenimiento>> ObtenerPaginacionAsync(int paginaNumero, int tamanoPagina);
    Task<int> ContarAsync();
    Task<OrdenMantenimiento?> ObtenerPorIdentificadorAsync(int id);
    Task<IEnumerable<OrdenMantenimiento>> ObtenerPorActivoAsync(int assetId);

    // Verificaciones para las reglas de negocio
    Task<string?> ObtenerEstadoActivoAsync(int assetId);
    Task<bool?> ObtenerSiTipoMantenimientoEsEmergenciaAsync(int maintenanceTypeId);
    Task<bool?> ObtenerSiProveedorEstaActivoAsync(int supplierId);
    Task<bool> TieneOrdenesAbiertasAsync(int assetId, IEnumerable<string> closedStatuses);
    Task<string?> ObtenerUltimoCodigoOrdenTrabajoAsync(string codePrefix);

    // Escritura
    Task AgregarAsync(OrdenMantenimiento orden);
}
