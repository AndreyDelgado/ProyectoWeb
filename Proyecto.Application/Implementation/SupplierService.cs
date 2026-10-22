using MapsterMapper;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Application.Implementation;

public class SupplierService : ISupplierService
{
    // Límite de registros por página
    public const int MaxPageSize = 50;

    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public SupplierService(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // READ

    public async Task<Result<IEnumerable<SupplierListDto>>> GetAllAsync()
    {
        var suppliers = await _repository.GetAllAsync();

        var data = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers);

        var result = Result<IEnumerable<SupplierListDto>>.Success(data);

        return result;
    }

    public async Task<Result<IEnumerable<SupplierListDto>>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > MaxPageSize)
            return Result<IEnumerable<SupplierListDto>>.Failure(400, "Bad Request",
                $"pageNumber debe ser 1 o mayor y pageSize debe estar entre 1 y {MaxPageSize}.");

        var suppliers = await _repository.GetPaginationAsync(pageNumber, pageSize);
        var totalRecords = await _repository.CountAsync();

        var data = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers);

        var result = Result<IEnumerable<SupplierListDto>>.Paged(data, pageNumber, pageSize, totalRecords);

        return result;
    }

    public async Task<Result<SupplierDetailDto>> GetByIdAsync(int id)
    {
        var supplier = await _repository.GetByIdAsync(id);

        if (supplier is null)
            return Result<SupplierDetailDto>.Failure(404, "Not Found", $"El proveedor {id} no existe.");

        var data = _mapper.Map<SupplierDetailDto>(supplier);

        var result = Result<SupplierDetailDto>.Success(data);

        return result;
    }

    // CREATE

    public async Task<Result<SupplierDetailDto>> AddAsync(SupplierCreateDto supplier)
    {
        // Regla de negocio: la cédula jurídica no se puede repetir
        if (await _repository.ExistsByTaxIdAsync(supplier.TaxId!))
            return Result<SupplierDetailDto>.Failure(409, "Conflict",
                $"Ya existe un proveedor con la cédula jurídica {supplier.TaxId}.");

        var entity = _mapper.Map<Supplier>(supplier);
        entity.IsActive = true;                    // Todo proveedor nuevo se crea activo

        await _repository.AddAsync(entity);        // SQL Server asigna el Id

        var data = _mapper.Map<SupplierDetailDto>(entity);

        var result = Result<SupplierDetailDto>.Success(data, 201, "Created", "Registro guardado.");

        return result;
    }

    // UPDATE

    public async Task<Result<SupplierDetailDto>> UpdateAsync(int id, SupplierUpdateDto supplier)
    {
        if (!await _repository.ExistsAsync(id))
            return Result<SupplierDetailDto>.Failure(404, "Not Found", $"El proveedor {id} no existe.");

        // Misma regla, excluyendo al proveedor que se edita
        if (await _repository.ExistsByTaxIdAsync(supplier.TaxId!, id))
            return Result<SupplierDetailDto>.Failure(409, "Conflict",
                $"Ya existe un proveedor con la cédula jurídica {supplier.TaxId}.");

        var entity = _mapper.Map<Supplier>(supplier);
        entity.Id = id;                            // El Id se toma de la ruta, no del body

        await _repository.UpdateAsync(entity);

        var data = _mapper.Map<SupplierDetailDto>(entity);

        var result = Result<SupplierDetailDto>.Success(data, 200, "OK", "Registro actualizado.");

        return result;
    }

    // DELETE

    public async Task<Result<int>> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            return Result<int>.Failure(404, "Not Found", $"El proveedor {id} no existe.");

        // Regla de negocio: no se elimina un proveedor con activos u órdenes de mantenimiento asociados
        if (await _repository.HasRelatedRecordsAsync(id))
            return Result<int>.Failure(409, "Conflict",
                $"El proveedor {id} tiene activos u órdenes de mantenimiento asociados y no se puede eliminar.");

        await _repository.DeleteAsync(id);

        var result = Result<int>.Success(id, 200, "OK", "Registro eliminado.");

        return result;
    }
}
