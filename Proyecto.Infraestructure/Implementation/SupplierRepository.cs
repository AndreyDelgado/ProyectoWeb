using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Infraestructure.Implementation;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    // Consultas de lectura sin seguimiento de cambios (AsNoTracking)

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        var result = await _context.Suppliers
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Supplier>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var result = await _context.Suppliers
            .AsNoTracking()
            .OrderBy(s => s.Id)                    // Orden necesario para Skip/Take
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    // Cantidad total de registros, utilizada en la paginación
    public async Task<int> CountAsync()
    {
        var result = await _context.Suppliers.CountAsync();

        return result;
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        var result = await _context.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        return result;
    }

    // Verificaciones de existencia mediante AnyAsync

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await _context.Suppliers
            .AnyAsync(s => s.Id == id);

        return result;
    }

    // excludeId: Id del proveedor que se excluye de la verificación al editar
    public async Task<bool> ExistsByTaxIdAsync(string taxId, int? excludeId = null)
    {
        var result = await _context.Suppliers
            .AnyAsync(s => s.TaxId == taxId
                        && (excludeId == null || s.Id != excludeId));

        return result;
    }

    // Reúne en un solo lugar todas las relaciones que impiden eliminar un proveedor
    public async Task<bool> HasRelatedRecordsAsync(int id)
    {
        var result = await _context.Suppliers
            .Where(s => s.Id == id)
            .AnyAsync(s => s.Assets.Any() || s.MaintenanceOrders.Any());

        return result;
    }

    // Operaciones de escritura

    public async Task AddAsync(Supplier supplier)
    {
        // Id generado por la base de datos (IDENTITY)
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        // Actualiza todas las columnas de la entidad
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        // Eliminación directa en la base de datos, sin cargar la entidad
        await _context.Suppliers
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();
    }
}
