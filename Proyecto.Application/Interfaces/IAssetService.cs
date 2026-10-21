using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.PatternResult;

namespace Proyecto.Application.Interfaces;

public interface IAssetService
{
    Task<Result<IEnumerable<AssetListDto>>> GetAllAsync();
    Task<Result<IEnumerable<AssetListDto>>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<Result<AssetDetailDto>> GetByIdAsync(int id);
    Task<Result<AssetDetailDto>> AddAsync(AssetCreateDto asset);
    Task<Result<AssetDetailDto>> UpdateAsync(int id, AssetUpdateDto asset);
    Task<Result<AssetDetailDto>> UpdateStatusAsync(int id, AssetStatusUpdateDto status);
    Task<Result<int>> DeleteAsync(int id);
    Task<Result<string>> UploadImageAsync(int id, Stream content, string fileName, long length);
    Task<Result<int>> DeleteImageAsync(int id);
}
