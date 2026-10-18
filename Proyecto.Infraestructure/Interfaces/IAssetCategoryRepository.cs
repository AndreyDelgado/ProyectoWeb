using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Interfaces;

public interface IAssetCategoryRepository
{
    // Lectura
    Task<IEnumerable<AssetCategory>> GetAllAsync();
    Task<IEnumerable<AssetCategory>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task<AssetCategory?> GetByIdAsync(int id);

    // Verificaciones para las reglas de negocio
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    Task<bool> ExistsByCodePrefixAsync(string codePrefix, int? excludeId = null);
    Task<bool> HasAssetsAsync(int id);

    // Escritura
    Task AddAsync(AssetCategory category);
    Task UpdateAsync(AssetCategory category);
    Task DeleteAsync(int id);
}
