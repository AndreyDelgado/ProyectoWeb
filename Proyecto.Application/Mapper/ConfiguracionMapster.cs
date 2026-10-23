using Mapster;
using Proyecto.Domain.DTO;
using Proyecto.Domain.Entities;

namespace Proyecto.Application.Mapper;

public static class ConfiguracionMapster
{
    // Ruta pública donde se sirven las imágenes de los activos
    public const string UrlImagenesActivos = "/images/assets/";

    // Imagen que se muestra cuando el activo no tiene una propia
    public const string ImagenPredeterminadaActivo = "default.png";

    public static void RegistrarMapeos()
    {
        // CategoriaActivo
        // Las propiedades de entidad a DTO de salida coinciden por nombre y no requieren configuración.
        // En los DTOs de entrada se ignora Identificador para que no se asigne desde el cliente.
        TypeAdapterConfig<CategoriaActivoCrearDto, CategoriaActivo>
            .NewConfig()
            .Ignore(dest => dest.Identificador);              // Generado por la base de datos (IDENTITY)

        TypeAdapterConfig<CategoriaActivoActualizarDto, CategoriaActivo>
            .NewConfig()
            .Ignore(dest => dest.Identificador);              // Se toma del parámetro de la ruta

        // Activo
        // Los nombres de las relaciones requieren Include en el repositorio.
        TypeAdapterConfig<Activo, ActivoListaDto>
            .NewConfig()
            .Map(dest => dest.NombreCategoria, src => src.Categoria.Nombre)
            .Map(dest => dest.NombreUbicacion, src => src.Ubicacion.Nombre)
            .Map(dest => dest.UrlImagen, src => UrlImagenesActivos + (string.IsNullOrEmpty(src.Imagen) ? ImagenPredeterminadaActivo : src.Imagen));

        TypeAdapterConfig<Activo, ActivoDetalleDto>
            .NewConfig()
            .Map(dest => dest.NombreCategoria, src => src.Categoria.Nombre)
            .Map(dest => dest.NombreUbicacion, src => src.Ubicacion.Nombre)
            .Map(dest => dest.NombreProveedor, src => src.Proveedor == null ? null : src.Proveedor.Nombre)
            .Map(dest => dest.UrlImagen, src => UrlImagenesActivos + (string.IsNullOrEmpty(src.Imagen) ? ImagenPredeterminadaActivo : src.Imagen));

        // En los DTOs de entrada se ignoran los campos que asigna el servidor
        TypeAdapterConfig<ActivoCrearDto, Activo>
            .NewConfig()
            .Ignore(            dest => dest.Identificador,
            dest => dest.CodigoActivo,
            dest => dest.Estado,
            dest => dest.HorasUsoAlUltimoMantenimiento,
            dest => dest.CreadoEn,
            dest => dest.Categoria,
            dest => dest.Ubicacion,
            dest => dest.Proveedor!,
            dest => dest.OrdenesMantenimiento);

        TypeAdapterConfig<ActivoActualizarDto, Activo>
            .NewConfig()
            .Ignore(            dest => dest.Identificador,
            dest => dest.CodigoActivo,
            dest => dest.Estado,
            dest => dest.Imagen!,
            dest => dest.HorasUsoAlUltimoMantenimiento,
            dest => dest.CreadoEn,
            dest => dest.Categoria,
            dest => dest.Ubicacion,
            dest => dest.Proveedor!,
            dest => dest.OrdenesMantenimiento);

        // Proveedor
        // Las propiedades de entidad a DTO de salida coinciden por nombre y no requieren configuración.
        TypeAdapterConfig<ProveedorCrearDto, Proveedor>
            .NewConfig()
            .Ignore(dest => dest.Identificador,                // Generado por la base de datos (IDENTITY)
                    dest => dest.EstaActivo,          // Lo asigna el servicio
                    dest => dest.Activos,
                    dest => dest.OrdenesMantenimiento);

        TypeAdapterConfig<ProveedorActualizarDto, Proveedor>
            .NewConfig()
            .Ignore(dest => dest.Identificador,                // Se toma del parámetro de la ruta
                    dest => dest.Activos,
                    dest => dest.OrdenesMantenimiento);

        // OrdenMantenimiento
        // Los datos del activo, el tipo y el proveedor requieren Include en el repositorio.
        TypeAdapterConfig<OrdenMantenimiento, OrdenMantenimientoListaDto>
            .NewConfig()
            .Map(dest => dest.CodigoActivo, src => src.Activo.CodigoActivo)
            .Map(dest => dest.NombreActivo, src => src.Activo.Nombre)
            .Map(dest => dest.NombreTipoMantenimiento, src => src.TipoMantenimiento.Nombre)
            .Map(dest => dest.NombreProveedor, src => src.Proveedor == null ? null : src.Proveedor.Nombre);

        TypeAdapterConfig<OrdenMantenimiento, OrdenMantenimientoDetalleDto>
            .NewConfig()
            .Map(dest => dest.CodigoActivo, src => src.Activo.CodigoActivo)
            .Map(dest => dest.NombreActivo, src => src.Activo.Nombre)
            .Map(dest => dest.NombreTipoMantenimiento, src => src.TipoMantenimiento.Nombre)
            .Map(dest => dest.EsEmergencia, src => src.TipoMantenimiento.EsEmergencia)
            .Map(dest => dest.NombreProveedor, src => src.Proveedor == null ? null : src.Proveedor.Nombre);

        // En el DTO de entrada se ignoran los campos que asigna el servidor o que se definen en etapas posteriores
        TypeAdapterConfig<OrdenMantenimientoCrearDto, OrdenMantenimiento>
            .NewConfig()
            .Ignore(dest => dest.Identificador,
                    dest => dest.CodigoOrdenTrabajo,
                    dest => dest.Estado,
                    dest => dest.SolicitadoEn,
                    dest => dest.ProgramadoPara!,
                    dest => dest.IniciadoEn!,
                    dest => dest.CerradoEn!,
                    dest => dest.CostoTotal!,
                    dest => dest.Hallazgos!,
                    dest => dest.CreadoEn,
                    dest => dest.Activo,
                    dest => dest.TipoMantenimiento,
                    dest => dest.Proveedor!);
    }
}
