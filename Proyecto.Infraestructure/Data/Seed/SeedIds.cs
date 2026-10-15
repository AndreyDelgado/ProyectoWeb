namespace Proyecto.Infraestructure.Data.Seed;

public static class SeedIds
{
    public static class AssetCategories
    {
        public const int ComputerEquipment = 1;
        public const int Furniture = 2;
        public const int IndustrialEquipment = 3;
    }

    public static class Locations
    {
        public const int MainBuilding = 1;
        public const int Workshop = 2;
        public const int Warehouse = 3;
    }

    public static class Suppliers
    {
        public const int TechnicalServices = 1;
        public const int IndustrialMaintenance = 2;
    }

    public static class Assets
    {
        public const int AdministrationLaptop = 1;
        public const int BenchDrill = 2;
        public const int ReceptionDesk = 3;
    }

    public static class MaintenanceTypes
    {
        public const int Preventive = 1;
        public const int Corrective = 2;
        public const int Inspection = 3;
    }

    public static class MaintenanceOrders
    {
        public const int LaptopPreventiveMaintenance = 1;
        public const int DrillSafetyInspection = 2;
        public const int DeskStructureReview = 3;
    }
}