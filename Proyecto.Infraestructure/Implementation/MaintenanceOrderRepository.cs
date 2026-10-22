using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Infraestructure.Implementation;

public class MaintenanceOrderRepository : IMaintenanceOrderRepository
{
    private readonly AppDbContext _context;

    public MaintenanceOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    // Consultas de lectura sin seguimiento de cambios (AsNoTracking)

    public async Task<IEnumerable<MaintenanceOrder>> GetAllAsync()
    {
        var result = await _context.MaintenanceOrders
            .AsNoTracking()
            .Include(o => o.Asset)
            .Include(o => o.MaintenanceType)
            .Include(o => o.Supplier)
            .OrderBy(o => o.Id)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<MaintenanceOrder>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var result = await _context.MaintenanceOrders
            .AsNoTracking()
            .Include(o => o.Asset)
            .Include(o => o.MaintenanceType)
            .Include(o => o.Supplier)
            .OrderBy(o => o.Id)                    // Orden necesario para Skip/Take
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    // Cantidad total de registros, utilizada en la paginación
    public async Task<int> CountAsync()
    {
        var result = await _context.MaintenanceOrders.CountAsync();

        return result;
    }

    public async Task<MaintenanceOrder?> GetByIdAsync(int id)
    {
        var result = await _context.MaintenanceOrders
            .AsNoTracking()
            .Include(o => o.Asset)
            .Include(o => o.MaintenanceType)
            .Include(o => o.Supplier)
            .FirstOrDefaultAsync(o => o.Id == id);

        return result;
    }

    public async Task<IEnumerable<MaintenanceOrder>> GetByAssetAsync(int assetId)
    {
        var result = await _context.MaintenanceOrders
            .AsNoTracking()
            .Include(o => o.Asset)
            .Include(o => o.MaintenanceType)
            .Include(o => o.Supplier)
            .Where(o => o.AssetId == assetId)
            .OrderByDescending(o => o.RequestedAt)
            .ThenByDescending(o => o.Id)           // Desempate para órdenes con la misma fecha
            .ToListAsync();

        return result;
    }

    // Verificaciones: cada método devuelve null si el registro no existe

    public async Task<string?> GetAssetStatusAsync(int assetId)
    {
        var result = await _context.Assets
            .Where(a => a.Id == assetId)
            .Select(a => a.Status)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool?> GetMaintenanceTypeIsEmergencyAsync(int maintenanceTypeId)
    {
        var result = await _context.MaintenanceTypes
            .Where(t => t.Id == maintenanceTypeId)
            .Select(t => (bool?)t.IsEmergency)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool?> GetSupplierIsActiveAsync(int supplierId)
    {
        var result = await _context.Suppliers
            .Where(s => s.Id == supplierId)
            .Select(s => (bool?)s.IsActive)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> HasOpenOrdersAsync(int assetId, IEnumerable<string> closedStatuses)
    {
        var result = await _context.MaintenanceOrders
            .AnyAsync(o => o.AssetId == assetId && !closedStatuses.Contains(o.Status));

        return result;
    }

    // El prefijo incluye el año ("OT-2026-"), por lo que el consecutivo se reinicia cada año
    public async Task<string?> GetLastWorkOrderCodeAsync(string codePrefix)
    {
        var result = await _context.MaintenanceOrders
            .Where(o => o.WorkOrderCode.StartsWith(codePrefix))
            .OrderByDescending(o => o.WorkOrderCode)
            .Select(o => o.WorkOrderCode)
            .FirstOrDefaultAsync();

        return result;
    }

    // Operaciones de escritura

    public async Task AddAsync(MaintenanceOrder order)
    {
        // Id generado por la base de datos (IDENTITY)
        _context.MaintenanceOrders.Add(order);
        await _context.SaveChangesAsync();
    }
}
