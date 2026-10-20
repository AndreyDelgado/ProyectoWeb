using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Infraestructure.Implementation;

public class AssetRepository : IAssetRepository
{
    private readonly AppDbContext _context;

    public AssetRepository(AppDbContext context)
    {
        _context = context;
    }

    // Consultas de lectura sin seguimiento de cambios (AsNoTracking)

    public async Task<IEnumerable<Asset>> GetAllAsync()
    {
        var result = await _context.Assets
            .AsNoTracking()
            .Include(a => a.Category)
            .Include(a => a.Location)
            .OrderBy(a => a.Id)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Asset>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var result = await _context.Assets
            .AsNoTracking()
            .Include(a => a.Category)
            .Include(a => a.Location)
            .OrderBy(a => a.Id)                    // Orden necesario para Skip/Take
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    // Cantidad total de registros, utilizada en la paginación
    public async Task<int> CountAsync()
    {
        var result = await _context.Assets.CountAsync();

        return result;
    }

    public async Task<Asset?> GetByIdAsync(int id)
    {
        var result = await _context.Assets
            .AsNoTracking()
            .Include(a => a.Category)
            .Include(a => a.Location)
            .Include(a => a.Supplier)
            .FirstOrDefaultAsync(a => a.Id == id);

        return result;
    }

    // Con seguimiento de cambios: SaveChangesAsync actualiza solo las columnas modificadas
    public async Task<Asset?> GetForUpdateAsync(int id)
    {
        var result = await _context.Assets
            .FirstOrDefaultAsync(a => a.Id == id);

        return result;
    }

    // Verificaciones de existencia

    // Devuelve null si la categoría no existe
    public async Task<string?> GetCategoryCodePrefixAsync(int categoryId)
    {
        var result = await _context.AssetCategories
            .Where(c => c.Id == categoryId)
            .Select(c => c.CodePrefix)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> LocationExistsAsync(int locationId)
    {
        var result = await _context.Locations
            .AnyAsync(l => l.Id == locationId);

        return result;
    }

    public async Task<bool> SupplierExistsAsync(int supplierId)
    {
        var result = await _context.Suppliers
            .AnyAsync(s => s.Id == supplierId);

        return result;
    }

    // excludeId: Id del activo que se excluye de la verificación al editar
    public async Task<bool> ExistsBySerialNumberAsync(string serialNumber, int? excludeId = null)
    {
        var result = await _context.Assets
            .AnyAsync(a => a.SerialNumber == serialNumber
                        && (excludeId == null || a.Id != excludeId));

        return result;
    }

    public async Task<bool> HasOpenOrdersAsync(int id, IEnumerable<string> closedStatuses)
    {
        var result = await _context.MaintenanceOrders
            .AnyAsync(o => o.AssetId == id && !closedStatuses.Contains(o.Status));

        return result;
    }

    // Incluye los activos dados de baja, para que un código no se vuelva a asignar
    public async Task<string?> GetLastAssetCodeAsync(string codePrefix)
    {
        var result = await _context.Assets
            .Where(a => a.AssetCode.StartsWith(codePrefix + "-"))
            .OrderByDescending(a => a.AssetCode)
            .Select(a => a.AssetCode)
            .FirstOrDefaultAsync();

        return result;
    }

    // Operaciones de escritura

    public async Task AddAsync(Asset asset)
    {
        // Id generado por la base de datos (IDENTITY)
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
