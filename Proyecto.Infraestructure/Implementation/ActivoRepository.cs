using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Infraestructure.Implementation;

public class ActivoRepository : IActivoRepository
{
    private readonly ContextoAplicacion _contexto;

    public ActivoRepository(ContextoAplicacion contexto)
    {
        _contexto = contexto;
    }

    // Consultas de lectura sin seguimiento de cambios (AsNoTracking)

    public async Task<IEnumerable<Activo>> ObtenerTodosAsync()
    {
        var resultado = await _contexto.Activos
            .AsNoTracking()
            .Include(a => a.Categoria)
            .Include(a => a.Ubicacion)
            .OrderBy(a => a.Identificador)
            .ToListAsync();

        return resultado;
    }

    public async Task<IEnumerable<Activo>> ObtenerPaginacionAsync(int paginaNumero, int tamanoPagina)
    {
        var resultado = await _contexto.Activos
            .AsNoTracking()
            .Include(a => a.Categoria)
            .Include(a => a.Ubicacion)
            .OrderBy(a => a.Identificador)                    // Orden necesario para Skip/Take
            .Skip((paginaNumero - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();

        return resultado;
    }

    // Cantidad total de registros, utilizada en la paginación
    public async Task<int> ContarAsync()
    {
        var resultado = await _contexto.Activos.CountAsync();

        return resultado;
    }

    public async Task<Activo?> ObtenerPorIdentificadorAsync(int id)
    {
        var resultado = await _contexto.Activos
            .AsNoTracking()
            .Include(a => a.Categoria)
            .Include(a => a.Ubicacion)
            .Include(a => a.Proveedor)
            .FirstOrDefaultAsync(a => a.Identificador == id);

        return resultado;
    }

    // Con seguimiento de cambios: SaveChangesAsync actualiza solo las columnas modificadas
    public async Task<Activo?> ObtenerParaActualizarAsync(int id)
    {
        var resultado = await _contexto.Activos
            .FirstOrDefaultAsync(a => a.Identificador == id);

        return resultado;
    }

    // Verificaciones de existencia

    // Devuelve null si la categoría no existe
    public async Task<string?> ObtenerPrefijoCodigoCategoriaAsync(int categoryId)
    {
        var resultado = await _contexto.CategoriasActivos
            .Where(c => c.Identificador == categoryId)
            .Select(c => c.PrefijoCodigo)
            .FirstOrDefaultAsync();

        return resultado;
    }

    public async Task<bool> ExisteUbicacionAsync(int locationId)
    {
        var resultado = await _contexto.Ubicaciones
            .AnyAsync(l => l.Identificador == locationId);

        return resultado;
    }

    public async Task<bool> ExisteProveedorAsync(int supplierId)
    {
        var resultado = await _contexto.Proveedores
            .AnyAsync(s => s.Identificador == supplierId);

        return resultado;
    }

    // excludeId: Identificador del activo que se excluye de la verificación al editar
    public async Task<bool> ExistePorNumeroSerieAsync(string serialNumber, int? excludeId = null)
    {
        var resultado = await _contexto.Activos
            .AnyAsync(a => a.NumeroSerie == serialNumber
                        && (excludeId == null || a.Identificador != excludeId));

        return resultado;
    }

    public async Task<bool> TieneOrdenesAbiertasAsync(int id, IEnumerable<string> closedStatuses)
    {
        var resultado = await _contexto.OrdenesMantenimiento
            .AnyAsync(o => o.ActivoId == id && !closedStatuses.Contains(o.Estado));

        return resultado;
    }

    // Incluye los activos dados de baja, para que un código no se vuelva a asignar
    public async Task<string?> ObtenerUltimoCodigoActivoAsync(string codePrefix)
    {
        var resultado = await _contexto.Activos
            .Where(a => a.CodigoActivo.StartsWith(codePrefix + "-"))
            .OrderByDescending(a => a.CodigoActivo)
            .Select(a => a.CodigoActivo)
            .FirstOrDefaultAsync();

        return resultado;
    }

    // Operaciones de escritura
    public async Task AgregarAsync(Activo activo)
    {
        // Identificador generado por la base de datos (IDENTITY)
        _contexto.Activos.Add(activo);
        await _contexto.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _contexto.SaveChangesAsync();
    }
}
