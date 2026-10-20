using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Interfaces;

public interface IAssetRepository
{
    // Lectura
    Task<IEnumerable<Asset>> GetAllAsync();
    Task<IEnumerable<Asset>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task<Asset?> GetByIdAsync(int id);
    Task<Asset?> GetForUpdateAsync(int id);

    // Verificaciones para las reglas de negocio
    Task<string?> GetCategoryCodePrefixAsync(int categoryId);
    Task<bool> LocationExistsAsync(int locationId);
    Task<bool> SupplierExistsAsync(int supplierId);
    Task<bool> ExistsBySerialNumberAsync(string serialNumber, int? excludeId = null);
    Task<bool> HasOpenOrdersAsync(int id, IEnumerable<string> closedStatuses);
    Task<string?> GetLastAssetCodeAsync(string codePrefix);

    // Escritura
    Task AddAsync(Asset asset);
    Task SaveChangesAsync();
}
