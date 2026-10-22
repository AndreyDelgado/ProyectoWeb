using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Interfaces;

public interface IMaintenanceOrderRepository
{
    // Lectura
    Task<IEnumerable<MaintenanceOrder>> GetAllAsync();
    Task<IEnumerable<MaintenanceOrder>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task<MaintenanceOrder?> GetByIdAsync(int id);
    Task<IEnumerable<MaintenanceOrder>> GetByAssetAsync(int assetId);

    // Verificaciones para las reglas de negocio
    Task<string?> GetAssetStatusAsync(int assetId);
    Task<bool?> GetMaintenanceTypeIsEmergencyAsync(int maintenanceTypeId);
    Task<bool?> GetSupplierIsActiveAsync(int supplierId);
    Task<bool> HasOpenOrdersAsync(int assetId, IEnumerable<string> closedStatuses);
    Task<string?> GetLastWorkOrderCodeAsync(string codePrefix);

    // Escritura
    Task AddAsync(MaintenanceOrder order);
}
