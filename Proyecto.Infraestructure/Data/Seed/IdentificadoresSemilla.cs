namespace Proyecto.Infraestructure.Data.Seed;

public static class IdentificadoresSemilla
{
    public static class CategoriasActivos
    {
        public const int EquipoComputo = 1;
        public const int Mobiliario = 2;
        public const int EquipoIndustrial = 3;
    }

    public static class Ubicaciones
    {
        public const int EdificioPrincipal = 1;
        public const int Taller = 2;
        public const int Bodega = 3;
    }

    public static class Proveedores
    {
        public const int ServiciosTecnicos = 1;
        public const int MantenimientoIndustrial = 2;
    }

    public static class Activos
    {
        public const int LaptopAdministracion = 1;
        public const int TaladroBanco = 2;
        public const int EscritorioRecepcion = 3;
    }

    public static class TiposMantenimiento
    {
        public const int Preventivo = 1;
        public const int Correctivo = 2;
        public const int Inspeccion = 3;
    }

    public static class OrdenesMantenimiento
    {
        public const int MantenimientoPreventivoLaptop = 1;
        public const int InspeccionSeguridadTaladro = 2;
        public const int RevisionEstructuraEscritorio = 3;
    }
}