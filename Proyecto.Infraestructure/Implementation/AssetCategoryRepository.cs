using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Infraestructure.Implementation;

public class AssetCategoryRepository : IAssetCategoryRepository
{
    private readonly AppDbContext _context;

    public AssetCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    // Consultas de lectura sin seguimiento de cambios (AsNoTracking)

    public async Task<IEnumerable<AssetCategory>> GetAllAsync()
    {
        var result = await _context.AssetCategories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<AssetCategory>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var result = await _context.AssetCategories
            .AsNoTracking()
            .OrderBy(c => c.Id)                    // Orden necesario para Skip/Take
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    // Cantidad total de registros, utilizada en la paginación
    public async Task<int> CountAsync()
    {
        var result = await _context.AssetCategories.CountAsync();

        return result;
    }

    public async Task<AssetCategory?> GetByIdAsync(int id)
    {
        var result = await _context.AssetCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return result;
    }

    // Verificaciones de existencia mediante AnyAsync

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await _context.AssetCategories
            .AnyAsync(c => c.Id == id);

        return result;
    }

    // excludeId: Id de la categoría que se excluye de la verificación al editar
    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        var result = await _context.AssetCategories
            .AnyAsync(c => c.Name == name
                        && (excludeId == null || c.Id != excludeId));

        return result;
    }

    // excludeId: Id de la categoría que se excluye de la verificación al editar
    public async Task<bool> ExistsByCodePrefixAsync(string codePrefix, int? excludeId = null)
    {
        var result = await _context.AssetCategories
            .AnyAsync(c => c.CodePrefix == codePrefix
                        && (excludeId == null || c.Id != excludeId));

        return result;
    }

    public async Task<bool> HasAssetsAsync(int id)
    {
        var result = await _context.Assets
            .AnyAsync(a => a.CategoryId == id);

        return result;
    }

    // Operaciones de escritura

    public async Task AddAsync(AssetCategory category)
    {
        // Id generado por la base de datos (IDENTITY)
        _context.AssetCategories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AssetCategory category)
    {
        // Actualiza todas las columnas de la entidad
        _context.AssetCategories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        // Eliminación directa en la base de datos, sin cargar la entidad
        await _context.AssetCategories
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();
    }
}
