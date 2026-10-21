using Mapster;
using Proyecto.Domain.DTO;
using Proyecto.Domain.Entities;

namespace Proyecto.Application.Mapper;

public static class MapsterConfig
{
    // Ruta pública donde se sirven las imágenes de los activos
    public const string AssetImagesUrl = "/images/assets/";

    // Imagen que se muestra cuando el activo no tiene una propia
    public const string DefaultAssetImage = "default.png";

    public static void RegisterMaps()
    {
        // AssetCategory
        // Las propiedades de entidad a DTO de salida coinciden por nombre y no requieren configuración.
        // En los DTOs de entrada se ignora Id para que no se asigne desde el cliente.
        TypeAdapterConfig<AssetCategoryCreateDto, AssetCategory>
            .NewConfig()
            .Ignore(dest => dest.Id);              // Generado por la base de datos (IDENTITY)

        TypeAdapterConfig<AssetCategoryUpdateDto, AssetCategory>
            .NewConfig()
            .Ignore(dest => dest.Id);              // Se toma del parámetro de la ruta

        // Asset
        // Los nombres de las relaciones requieren Include en el repositorio.
        TypeAdapterConfig<Asset, AssetListDto>
            .NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.LocationName, src => src.Location.Name)
            .Map(dest => dest.ImageUrl, src => AssetImagesUrl + (string.IsNullOrEmpty(src.Image) ? DefaultAssetImage : src.Image));

        TypeAdapterConfig<Asset, AssetDetailDto>
            .NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.LocationName, src => src.Location.Name)
            .Map(dest => dest.SupplierName, src => src.Supplier == null ? null : src.Supplier.Name)
            .Map(dest => dest.ImageUrl, src => AssetImagesUrl + (string.IsNullOrEmpty(src.Image) ? DefaultAssetImage : src.Image));

        // En los DTOs de entrada se ignoran los campos que asigna el servidor
        TypeAdapterConfig<AssetCreateDto, Asset>
            .NewConfig()
            .Ignore(dest => dest.Id,
                    dest => dest.AssetCode,
                    dest => dest.Status,
                    dest => dest.UsageHoursAtLastMaintenance,
                    dest => dest.CreatedAt,
                    dest => dest.Category,
                    dest => dest.Location,
                    dest => dest.Supplier!,
                    dest => dest.MaintenanceOrders);

        TypeAdapterConfig<AssetUpdateDto, Asset>
            .NewConfig()
            .Ignore(dest => dest.Id,
                    dest => dest.AssetCode,
                    dest => dest.Status,
                    dest => dest.Image!,
                    dest => dest.UsageHoursAtLastMaintenance,
                    dest => dest.CreatedAt,
                    dest => dest.Category,
                    dest => dest.Location,
                    dest => dest.Supplier!,
                    dest => dest.MaintenanceOrders);
    }
}
