using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Data.Seed;

public static class InicializadorCatalogos
{
    public static ModelBuilder SembrarCatalogos(this ModelBuilder constructorModelo)
    {
        constructorModelo.Entity<CategoriaActivo>().HasData(
            new CategoriaActivo { Identificador = IdentificadoresSemilla.CategoriasActivos.EquipoComputo, Nombre = "Equipo de cómputo", Descripcion = "Computadoras y periféricos 1234", UmbralPreventivoHoras = 2000, PrefijoCodigo = "COMP" },
            new CategoriaActivo { Identificador = IdentificadoresSemilla.CategoriasActivos.Mobiliario, Nombre = "Mobiliario", Descripcion = "Muebles y estaciones de trabajo", UmbralPreventivoHoras = 4000, PrefijoCodigo = "MOB" },
            new CategoriaActivo { Identificador = IdentificadoresSemilla.CategoriasActivos.EquipoIndustrial, Nombre = "Equipo industrial", Descripcion = "Máquinas y herramientas de producción", UmbralPreventivoHoras = 500, PrefijoCodigo = "IND" });

        constructorModelo.Entity<Ubicacion>().HasData(
            new Ubicacion { Identificador = IdentificadoresSemilla.Ubicaciones.EdificioPrincipal, Nombre = "Edificio principal", Descripcion = "Sede central" },
            new Ubicacion { Identificador = IdentificadoresSemilla.Ubicaciones.Taller, Nombre = "Taller", Descripcion = "Área de mantenimiento" },
            new Ubicacion { Identificador = IdentificadoresSemilla.Ubicaciones.Bodega, Nombre = "Bodega", Descripcion = "Almacenamiento de activos" });

        constructorModelo.Entity<Proveedor>().HasData(
            new Proveedor { Identificador = IdentificadoresSemilla.Proveedores.ServiciosTecnicos, Nombre = "Servicios Técnicos CR", IdentificacionFiscal = "3-101-000001", NombreContacto = "María Rodríguez", Telefono = "2200-1000", Correo = "servicio@example.com", Direccion = "San José" },
            new Proveedor { Identificador = IdentificadoresSemilla.Proveedores.MantenimientoIndustrial, Nombre = "Mantenimiento Industrial del Valle", IdentificacionFiscal = "3-101-000002", NombreContacto = "Carlos Méndez", Telefono = "2200-2000", Correo = "taller@example.com", Direccion = "Heredia" });

        constructorModelo.Entity<Activo>().HasData(
            new Activo { Identificador = IdentificadoresSemilla.Activos.LaptopAdministracion, CodigoActivo = "ACT-0001", Nombre = "Laptop de administración", Descripcion = "Equipo portátil de oficina", Marca = "Dell", Modelo = "Latitude 5440", NumeroSerie = "SN-DELL-0001", FechaAdquisicion = new DateTime(2024, 1, 15), CostoCompra = 625000m, MesesVidaUtil = 48, Estado = "Operational", HorasUso = 1800, HorasUsoAlUltimoMantenimiento = 0, Criticidad = "Medium", CategoriaActivoId = IdentificadoresSemilla.CategoriasActivos.EquipoComputo, UbicacionId = IdentificadoresSemilla.Ubicaciones.EdificioPrincipal, ProveedorId = IdentificadoresSemilla.Proveedores.ServiciosTecnicos, CreadoEn = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Activo { Identificador = IdentificadoresSemilla.Activos.TaladroBanco, CodigoActivo = "ACT-0002", Nombre = "Taladro de banco", Descripcion = "Taladro para trabajos de taller", Marca = "Bosch", Modelo = "GBM 13-2 RE", NumeroSerie = "SN-BOSCH-0002", FechaAdquisicion = new DateTime(2023, 8, 10), CostoCompra = 185000m, MesesVidaUtil = 72, Estado = "Operational", HorasUso = 950, HorasUsoAlUltimoMantenimiento = 600, Criticidad = "High", CategoriaActivoId = IdentificadoresSemilla.CategoriasActivos.EquipoIndustrial, UbicacionId = IdentificadoresSemilla.Ubicaciones.Taller, ProveedorId = IdentificadoresSemilla.Proveedores.MantenimientoIndustrial, CreadoEn = new DateTime(2023, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Activo { Identificador = IdentificadoresSemilla.Activos.EscritorioRecepcion, CodigoActivo = "ACT-0003", Nombre = "Escritorio de recepción", Descripcion = "Módulo de recepción", Marca = "Oficina CR", Modelo = "Recepción 180", FechaAdquisicion = new DateTime(2022, 3, 2), CostoCompra = 210000m, MesesVidaUtil = 120, Estado = "Operational", HorasUso = 0, HorasUsoAlUltimoMantenimiento = 0, Criticidad = "Low", CategoriaActivoId = IdentificadoresSemilla.CategoriasActivos.Mobiliario, UbicacionId = IdentificadoresSemilla.Ubicaciones.EdificioPrincipal, CreadoEn = new DateTime(2022, 3, 2, 0, 0, 0, DateTimeKind.Utc) });

        constructorModelo.Entity<TipoMantenimiento>().HasData(
            new TipoMantenimiento { Identificador = IdentificadoresSemilla.TiposMantenimiento.Preventivo, Nombre = "Preventivo", Descripcion = "Mantenimiento programado para prevenir fallas", IntervaloPredeterminadoDias = 90 },
            new TipoMantenimiento { Identificador = IdentificadoresSemilla.TiposMantenimiento.Correctivo, Nombre = "Correctivo", Descripcion = "Reparación de una falla reportada" },
            new TipoMantenimiento { Identificador = IdentificadoresSemilla.TiposMantenimiento.Inspeccion, Nombre = "Inspección", Descripcion = "Revisión general del estado del activo", IntervaloPredeterminadoDias = 180 });

        constructorModelo.Entity<OrdenMantenimiento>().HasData(
            new OrdenMantenimiento { Identificador = IdentificadoresSemilla.OrdenesMantenimiento.MantenimientoPreventivoLaptop, CodigoOrdenTrabajo = "OT-2026-0001", ActivoId = IdentificadoresSemilla.Activos.LaptopAdministracion, TipoMantenimientoId = IdentificadoresSemilla.TiposMantenimiento.Preventivo, ProveedorId = IdentificadoresSemilla.Proveedores.ServiciosTecnicos, Titulo = "Limpieza y revisión preventiva", Descripcion = "Limpieza interna, revisión de batería y diagnóstico", Motivo = "Mantenimiento preventivo según el intervalo de 90 días", Prioridad = "Medium", Estado = "Requested", SolicitadoEn = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc), ProgramadoPara = new DateTime(2026, 10, 15), CostoTotal = 25000m, CreadoEn = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc) },
            new OrdenMantenimiento { Identificador = IdentificadoresSemilla.OrdenesMantenimiento.InspeccionSeguridadTaladro, CodigoOrdenTrabajo = "OT-2026-0002", ActivoId = IdentificadoresSemilla.Activos.TaladroBanco, TipoMantenimientoId = IdentificadoresSemilla.TiposMantenimiento.Inspeccion, ProveedorId = IdentificadoresSemilla.Proveedores.MantenimientoIndustrial, Titulo = "Inspección de seguridad", Descripcion = "Verificar broca, cableado y fijación de la herramienta", Motivo = "Inspección periódica de seguridad del equipo de taller", Prioridad = "High", Estado = "Completed", SolicitadoEn = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc), ProgramadoPara = new DateTime(2026, 8, 20), IniciadoEn = new DateTime(2026, 8, 20, 8, 0, 0, DateTimeKind.Utc), CerradoEn = new DateTime(2026, 8, 20, 10, 0, 0, DateTimeKind.Utc), CostoTotal = 18000m, Hallazgos = "Equipo operativo; se ajustó la base", CreadoEn = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
            new OrdenMantenimiento { Identificador = IdentificadoresSemilla.OrdenesMantenimiento.RevisionEstructuraEscritorio, CodigoOrdenTrabajo = "OT-2026-0003", ActivoId = IdentificadoresSemilla.Activos.EscritorioRecepcion, TipoMantenimientoId = IdentificadoresSemilla.TiposMantenimiento.Preventivo, Titulo = "Revisión de estructura", Descripcion = "Ajustar uniones y comprobar estabilidad", Motivo = "Se reportan uniones flojas en el módulo de recepción", Prioridad = "Low", Estado = "Requested", SolicitadoEn = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc), ProgramadoPara = new DateTime(2026, 11, 5), CreadoEn = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc) });

        return constructorModelo;
    }
}