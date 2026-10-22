using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Interfaces;

public interface ISupplierRepository
{
    // Lectura
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<IEnumerable<Supplier>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task<Supplier?> GetByIdAsync(int id);

    // Verificaciones para las reglas de negocio
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByTaxIdAsync(string taxId, int? excludeId = null);
    Task<bool> HasRelatedRecordsAsync(int id);

    // Escritura
    Task AddAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
    Task DeleteAsync(int id);
}
