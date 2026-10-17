using MapsterMapper;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Application.Implementation;

public class AssetCategoryService : IAssetCategoryService
{
    // Límite de registros por página
    public const int MaxPageSize = 50;

    private readonly IAssetCategoryRepository _repository;
    private readonly IMapper _mapper;

    public AssetCategoryService(IAssetCategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // READ

    public async Task<Result<IEnumerable<AssetCategoryListDto>>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();

        var data = _mapper.Map<IEnumerable<AssetCategoryListDto>>(categories);

        var result = Result<IEnumerable<AssetCategoryListDto>>.Success(data);

        return result;
    }

    public async Task<Result<IEnumerable<AssetCategoryListDto>>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > MaxPageSize)
            return Result<IEnumerable<AssetCategoryListDto>>.Failure(400, "Bad Request",
                $"pageNumber debe ser 1 o mayor y pageSize debe estar entre 1 y {MaxPageSize}.");

        var categories = await _repository.GetPaginationAsync(pageNumber, pageSize);
        var totalRecords = await _repository.CountAsync();

        var data = _mapper.Map<IEnumerable<AssetCategoryListDto>>(categories);

        var result = Result<IEnumerable<AssetCategoryListDto>>.Paged(data, pageNumber, pageSize, totalRecords);

        return result;
    }

    public async Task<Result<AssetCategoryDetailDto>> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category is null)
            return Result<AssetCategoryDetailDto>.Failure(404, "Not Found", $"La categoría {id} no existe.");

        var data = _mapper.Map<AssetCategoryDetailDto>(category);

        var result = Result<AssetCategoryDetailDto>.Success(data);

        return result;
    }

    // CREATE

    public async Task<Result<AssetCategoryDetailDto>> AddAsync(AssetCategoryCreateDto category)
    {
        // Regla de negocio: el nombre no se puede repetir
        if (await _repository.ExistsByNameAsync(category.Name!))
            return Result<AssetCategoryDetailDto>.Failure(409, "Conflict",
                $"Ya existe una categoría con el nombre {category.Name}.");

        var entity = _mapper.Map<AssetCategory>(category);
        entity.IsActive = true;                    // Toda categoría nueva se crea activa

        await _repository.AddAsync(entity);        // SQL Server asigna el Id

        var data = _mapper.Map<AssetCategoryDetailDto>(entity);

        var result = Result<AssetCategoryDetailDto>.Success(data, 201, "Created", "Registro guardado.");

        return result;
    }

    // UPDATE

    public async Task<Result<AssetCategoryDetailDto>> UpdateAsync(int id, AssetCategoryUpdateDto category)
    {
        if (!await _repository.ExistsAsync(id))
            return Result<AssetCategoryDetailDto>.Failure(404, "Not Found", $"La categoría {id} no existe.");

        // Misma regla, excluyendo a la categoría que se edita
        if (await _repository.ExistsByNameAsync(category.Name!, id))
            return Result<AssetCategoryDetailDto>.Failure(409, "Conflict",
                $"Ya existe una categoría con el nombre {category.Name}.");

        var entity = _mapper.Map<AssetCategory>(category);
        entity.Id = id;                            // El Id se toma de la ruta, no del body

        await _repository.UpdateAsync(entity);

        var data = _mapper.Map<AssetCategoryDetailDto>(entity);

        var result = Result<AssetCategoryDetailDto>.Success(data, 200, "OK", "Registro actualizado.");

        return result;
    }

    // DELETE

    public async Task<Result<int>> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            return Result<int>.Failure(404, "Not Found", $"La categoría {id} no existe.");

        // Regla de negocio: no se elimina una categoría con activos asociados
        if (await _repository.HasAssetsAsync(id))
            return Result<int>.Failure(409, "Conflict",
                $"La categoría {id} tiene activos asociados y no se puede eliminar.");

        await _repository.DeleteAsync(id);

        var result = Result<int>.Success(id, 200, "OK", "Registro eliminado.");

        return result;
    }
}
