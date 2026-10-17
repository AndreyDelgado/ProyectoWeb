using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;

namespace Proyecto.Application.Interfaces;

public interface IAssetCategoryService
{
    Task<Result<IEnumerable<AssetCategoryListDto>>> GetAllAsync();
    Task<Result<IEnumerable<AssetCategoryListDto>>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<Result<AssetCategoryDetailDto>> GetByIdAsync(int id);
    Task<Result<AssetCategoryDetailDto>> AddAsync(AssetCategoryCreateDto category);
    Task<Result<AssetCategoryDetailDto>> UpdateAsync(int id, AssetCategoryUpdateDto category);
    Task<Result<int>> DeleteAsync(int id);
}
