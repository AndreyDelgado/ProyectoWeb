using MapsterMapper;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Application.Implementation;

public class MaintenanceOrderService : IMaintenanceOrderService
{
    // Límite de registros por página
    public const int MaxPageSize = 50;

    public const string StatusRequested = "Requested";

    private readonly IMaintenanceOrderRepository _repository;
    private readonly IMapper _mapper;

    public MaintenanceOrderService(IMaintenanceOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // READ

    public async Task<Result<IEnumerable<MaintenanceOrderListDto>>> GetAllAsync()
    {
        var orders = await _repository.GetAllAsync();

        var data = _mapper.Map<IEnumerable<MaintenanceOrderListDto>>(orders);

        var result = Result<IEnumerable<MaintenanceOrderListDto>>.Success(data);

        return result;
    }

    public async Task<Result<IEnumerable<MaintenanceOrderListDto>>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > MaxPageSize)
            return Result<IEnumerable<MaintenanceOrderListDto>>.Failure(400, "Bad Request",
                $"pageNumber debe ser 1 o mayor y pageSize debe estar entre 1 y {MaxPageSize}.");

        var orders = await _repository.GetPaginationAsync(pageNumber, pageSize);
        var totalRecords = await _repository.CountAsync();

        var data = _mapper.Map<IEnumerable<MaintenanceOrderListDto>>(orders);

        var result = Result<IEnumerable<MaintenanceOrderListDto>>.Paged(data, pageNumber, pageSize, totalRecords);

        return result;
    }

    public async Task<Result<MaintenanceOrderDetailDto>> GetByIdAsync(int id)
    {
        var order = await _repository.GetByIdAsync(id);

        if (order is null)
            return Result<MaintenanceOrderDetailDto>.Failure(404, "Not Found", $"La orden {id} no existe.");

        var data = _mapper.Map<MaintenanceOrderDetailDto>(order);

        var result = Result<MaintenanceOrderDetailDto>.Success(data);

        return result;
    }

    // Un activo sin órdenes devuelve una lista vacía; un activo inexistente, 404
    public async Task<Result<IEnumerable<MaintenanceOrderListDto>>> GetByAssetAsync(int assetId)
    {
        if (await _repository.GetAssetStatusAsync(assetId) is null)
            return Result<IEnumerable<MaintenanceOrderListDto>>.Failure(404, "Not Found", $"El activo {assetId} no existe.");

        var orders = await _repository.GetByAssetAsync(assetId);

        var data = _mapper.Map<IEnumerable<MaintenanceOrderListDto>>(orders);

        var result = Result<IEnumerable<MaintenanceOrderListDto>>.Success(data);

        return result;
    }

    // CREATE

    public async Task<Result<MaintenanceOrderDetailDto>> AddAsync(MaintenanceOrderCreateDto order)
    {
        var assetStatus = await _repository.GetAssetStatusAsync(order.AssetId);

        if (assetStatus is null)
            return Result<MaintenanceOrderDetailDto>.Failure(400, "Bad Request", $"El activo {order.AssetId} no existe.");

        var isEmergency = await _repository.GetMaintenanceTypeIsEmergencyAsync(order.MaintenanceTypeId);

        if (isEmergency is null)
            return Result<MaintenanceOrderDetailDto>.Failure(400, "Bad Request",
                $"El tipo de mantenimiento {order.MaintenanceTypeId} no existe.");

        bool? supplierIsActive = null;

        if (order.SupplierId is not null)
        {
            supplierIsActive = await _repository.GetSupplierIsActiveAsync(order.SupplierId.Value);

            if (supplierIsActive is null)
                return Result<MaintenanceOrderDetailDto>.Failure(400, "Bad Request", $"El proveedor {order.SupplierId} no existe.");
        }

        // Reglas de negocio sobre el estado de los registros relacionados

        if (assetStatus == AssetService.StatusDecommissioned)
            return Result<MaintenanceOrderDetailDto>.Failure(409, "Conflict",
                $"El activo {order.AssetId} está dado de baja y no puede recibir órdenes.");

        if (supplierIsActive == false)
            return Result<MaintenanceOrderDetailDto>.Failure(409, "Conflict", $"El proveedor {order.SupplierId} está inactivo.");

        // Un tipo de emergencia permite abrir otra orden aunque el activo ya esté en mantenimiento
        if (assetStatus == AssetService.StatusUnderMaintenance
            && isEmergency == false
            && await _repository.HasOpenOrdersAsync(order.AssetId, AssetService.ClosedOrderStatuses))
            return Result<MaintenanceOrderDetailDto>.Failure(409, "Conflict",
                $"El activo {order.AssetId} está en mantenimiento y tiene una orden sin cerrar; solo se permite una orden de emergencia.");

        var now = DateTime.UtcNow;

        var entity = _mapper.Map<MaintenanceOrder>(order);
        entity.WorkOrderCode = await GenerateWorkOrderCodeAsync(now.Year);
        entity.Status = StatusRequested;
        entity.RequestedAt = now;
        entity.CreatedAt = now;

        await _repository.AddAsync(entity);        // SQL Server asigna el Id

        // Se vuelve a consultar para incluir los datos del activo, el tipo y el proveedor
        var created = await _repository.GetByIdAsync(entity.Id);

        var data = _mapper.Map<MaintenanceOrderDetailDto>(created!);

        var result = Result<MaintenanceOrderDetailDto>.Success(data, 201, "Created", "Registro guardado.");

        return result;
    }

    // OT-{año}-{consecutivo de 4 dígitos}: OT-2026-0001, OT-2026-0002...
    private async Task<string> GenerateWorkOrderCodeAsync(int year)
    {
        var codePrefix = $"OT-{year}-";
        var lastCode = await _repository.GetLastWorkOrderCodeAsync(codePrefix);

        var lastNumber = lastCode is not null && int.TryParse(lastCode[codePrefix.Length..], out var number)
            ? number
            : 0;

        return $"{codePrefix}{lastNumber + 1:D4}";
    }
}
