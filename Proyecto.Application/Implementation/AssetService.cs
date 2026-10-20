using MapsterMapper;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Application.Implementation;

public class AssetService : IAssetService
{
    // Límite de registros por página
    public const int MaxPageSize = 50;

    public const string StatusDecommissioned = "Decommissioned";

    // Estados en los que una orden se considera cerrada; cualquier otro estado impide la baja del activo
    public static readonly string[] ClosedOrderStatuses = ["Completed", "Cancelled"];

    private readonly IAssetRepository _repository;
    private readonly IMapper _mapper;

    public AssetService(IAssetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // READ

    public async Task<Result<IEnumerable<AssetListDto>>> GetAllAsync()
    {
        var assets = await _repository.GetAllAsync();

        var data = _mapper.Map<IEnumerable<AssetListDto>>(assets);

        var result = Result<IEnumerable<AssetListDto>>.Success(data);

        return result;
    }

    public async Task<Result<IEnumerable<AssetListDto>>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > MaxPageSize)
            return Result<IEnumerable<AssetListDto>>.Failure(400, "Bad Request",
                $"pageNumber debe ser 1 o mayor y pageSize debe estar entre 1 y {MaxPageSize}.");

        var assets = await _repository.GetPaginationAsync(pageNumber, pageSize);
        var totalRecords = await _repository.CountAsync();

        var data = _mapper.Map<IEnumerable<AssetListDto>>(assets);

        var result = Result<IEnumerable<AssetListDto>>.Paged(data, pageNumber, pageSize, totalRecords);

        return result;
    }

    public async Task<Result<AssetDetailDto>> GetByIdAsync(int id)
    {
        var asset = await _repository.GetByIdAsync(id);

        if (asset is null)
            return Result<AssetDetailDto>.Failure(404, "Not Found", $"El activo {id} no existe.");

        var data = _mapper.Map<AssetDetailDto>(asset);

        var result = Result<AssetDetailDto>.Success(data);

        return result;
    }

    // CREATE

    public async Task<Result<AssetDetailDto>> AddAsync(AssetCreateDto asset)
    {
        var codePrefix = await _repository.GetCategoryCodePrefixAsync(asset.CategoryId);

        if (codePrefix is null)
            return Result<AssetDetailDto>.Failure(400, "Bad Request", $"La categoría {asset.CategoryId} no existe.");

        var error = await ValidateRelationsAsync(asset.LocationId, asset.SupplierId);

        if (error is not null)
            return Result<AssetDetailDto>.Failure(400, "Bad Request", error);

        // Regla de negocio: el número de serie no se puede repetir
        if (!string.IsNullOrWhiteSpace(asset.SerialNumber)
            && await _repository.ExistsBySerialNumberAsync(asset.SerialNumber))
            return Result<AssetDetailDto>.Failure(409, "Conflict",
                $"Ya existe un activo con el número de serie {asset.SerialNumber}.");

        var entity = _mapper.Map<Asset>(asset);
        entity.AssetCode = await GenerateAssetCodeAsync(codePrefix);
        entity.Status = "Operational";
        entity.UsageHoursAtLastMaintenance = 0;
        entity.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(entity);        // SQL Server asigna el Id

        // Se vuelve a consultar para incluir los nombres de categoría, ubicación y proveedor
        var created = await _repository.GetByIdAsync(entity.Id);

        var data = _mapper.Map<AssetDetailDto>(created!);

        var result = Result<AssetDetailDto>.Success(data, 201, "Created", "Registro guardado.");

        return result;
    }

    // UPDATE

    public async Task<Result<AssetDetailDto>> UpdateAsync(int id, AssetUpdateDto asset)
    {
        var entity = await _repository.GetForUpdateAsync(id);

        if (entity is null)
            return Result<AssetDetailDto>.Failure(404, "Not Found", $"El activo {id} no existe.");

        if (entity.Status == StatusDecommissioned)
            return Result<AssetDetailDto>.Failure(409, "Conflict",
                $"El activo {id} está dado de baja y no se puede editar.");

        if (await _repository.GetCategoryCodePrefixAsync(asset.CategoryId) is null)
            return Result<AssetDetailDto>.Failure(400, "Bad Request", $"La categoría {asset.CategoryId} no existe.");

        var error = await ValidateRelationsAsync(asset.LocationId, asset.SupplierId);

        if (error is not null)
            return Result<AssetDetailDto>.Failure(400, "Bad Request", error);

        // Evita violar la restricción CK_Assets_UsageHours de la base de datos
        if (asset.UsageHours < entity.UsageHoursAtLastMaintenance)
            return Result<AssetDetailDto>.Failure(400, "Bad Request",
                $"Las horas de uso no pueden ser menores que las registradas en el último mantenimiento ({entity.UsageHoursAtLastMaintenance}).");

        if (!string.IsNullOrWhiteSpace(asset.SerialNumber)
            && await _repository.ExistsBySerialNumberAsync(asset.SerialNumber, id))
            return Result<AssetDetailDto>.Failure(409, "Conflict",
                $"Ya existe un activo con el número de serie {asset.SerialNumber}.");

        _mapper.Map(asset, entity);

        await _repository.SaveChangesAsync();

        var updated = await _repository.GetByIdAsync(id);

        var data = _mapper.Map<AssetDetailDto>(updated!);

        var result = Result<AssetDetailDto>.Success(data, 200, "OK", "Registro actualizado.");

        return result;
    }

    public async Task<Result<AssetDetailDto>> UpdateStatusAsync(int id, AssetStatusUpdateDto status)
    {
        var entity = await _repository.GetForUpdateAsync(id);

        if (entity is null)
            return Result<AssetDetailDto>.Failure(404, "Not Found", $"El activo {id} no existe.");

        if (entity.Status == StatusDecommissioned)
            return Result<AssetDetailDto>.Failure(409, "Conflict",
                $"El activo {id} está dado de baja y no se puede cambiar de estado.");

        entity.Status = status.Status!;

        await _repository.SaveChangesAsync();

        var updated = await _repository.GetByIdAsync(id);

        var data = _mapper.Map<AssetDetailDto>(updated!);

        var result = Result<AssetDetailDto>.Success(data, 200, "OK", "Estado actualizado.");

        return result;
    }

    // DELETE

    // Baja lógica: la fila se conserva para mantener el historial de órdenes y el código del activo
    public async Task<Result<int>> DeleteAsync(int id)
    {
        var entity = await _repository.GetForUpdateAsync(id);

        if (entity is null)
            return Result<int>.Failure(404, "Not Found", $"El activo {id} no existe.");

        if (entity.Status == StatusDecommissioned)
            return Result<int>.Failure(409, "Conflict", $"El activo {id} ya está dado de baja.");

        // Regla de negocio: no se da de baja un activo con órdenes sin cerrar
        if (await _repository.HasOpenOrdersAsync(id, ClosedOrderStatuses))
            return Result<int>.Failure(409, "Conflict",
                $"El activo {id} tiene órdenes de mantenimiento sin cerrar y no se puede dar de baja.");

        entity.Status = StatusDecommissioned;

        await _repository.SaveChangesAsync();

        var result = Result<int>.Success(id, 200, "OK", "Activo dado de baja.");

        return result;
    }

    // La ubicación debe existir y el proveedor también, si se indica. Devuelve null si no hay error.
    private async Task<string?> ValidateRelationsAsync(int locationId, int? supplierId)
    {
        if (!await _repository.LocationExistsAsync(locationId))
            return $"La ubicación {locationId} no existe.";

        if (supplierId is not null && !await _repository.SupplierExistsAsync(supplierId.Value))
            return $"El proveedor {supplierId} no existe.";

        return null;
    }

    // Prefijo de la categoría + consecutivo de 4 dígitos: SRV-0001, SRV-0002...
    private async Task<string> GenerateAssetCodeAsync(string codePrefix)
    {
        var lastCode = await _repository.GetLastAssetCodeAsync(codePrefix);

        var lastNumber = lastCode is not null && int.TryParse(lastCode[(codePrefix.Length + 1)..], out var number)
            ? number
            : 0;

        return $"{codePrefix}-{lastNumber + 1:D4}";
    }
}
