using Mapster;
using Proyecto.Domain.DTO;
using Proyecto.Domain.Entities;

namespace Proyecto.Application.Mapper;

public static class MapsterConfig
{
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
    }
}
