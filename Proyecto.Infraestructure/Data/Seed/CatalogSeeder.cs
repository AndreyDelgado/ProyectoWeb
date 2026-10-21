using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Data.Seed;

public static class CatalogSeeder
{
    public static ModelBuilder SeedCatalogs(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetCategory>().HasData(
            new AssetCategory { Id = SeedIds.AssetCategories.ComputerEquipment, Name = "Equipo de cómputo", Description = "Computadoras y periféricos", PreventiveThresholdHours = 2000, CodePrefix = "COMP" },
            new AssetCategory { Id = SeedIds.AssetCategories.Furniture, Name = "Mobiliario", Description = "Muebles y estaciones de trabajo", PreventiveThresholdHours = 4000, CodePrefix = "MOB" },
            new AssetCategory { Id = SeedIds.AssetCategories.IndustrialEquipment, Name = "Equipo industrial", Description = "Máquinas y herramientas de producción", PreventiveThresholdHours = 500, CodePrefix = "IND" });

        modelBuilder.Entity<Location>().HasData(
            new Location { Id = SeedIds.Locations.MainBuilding, Name = "Edificio principal", Description = "Sede central" },
            new Location { Id = SeedIds.Locations.Workshop, Name = "Taller", Description = "Área de mantenimiento" },
            new Location { Id = SeedIds.Locations.Warehouse, Name = "Bodega", Description = "Almacenamiento de activos" });

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = SeedIds.Suppliers.TechnicalServices, Name = "Servicios Técnicos CR", TaxId = "3-101-000001", ContactName = "María Rodríguez", Phone = "2200-1000", Email = "servicio@example.com", Address = "San José" },
            new Supplier { Id = SeedIds.Suppliers.IndustrialMaintenance, Name = "Mantenimiento Industrial del Valle", TaxId = "3-101-000002", ContactName = "Carlos Méndez", Phone = "2200-2000", Email = "taller@example.com", Address = "Heredia" });

        modelBuilder.Entity<Asset>().HasData(
            new Asset { Id = SeedIds.Assets.AdministrationLaptop, AssetCode = "ACT-0001", Name = "Laptop de administración", Description = "Equipo portátil de oficina", Brand = "Dell", Model = "Latitude 5440", SerialNumber = "SN-DELL-0001", AcquiredOn = new DateTime(2024, 1, 15), PurchaseCost = 625000m, UsefulLifeMonths = 48, Status = "Operational", UsageHours = 1800, UsageHoursAtLastMaintenance = 0, Criticality = "Medium", CategoryId = SeedIds.AssetCategories.ComputerEquipment, LocationId = SeedIds.Locations.MainBuilding, SupplierId = SeedIds.Suppliers.TechnicalServices, CreatedAt = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Asset { Id = SeedIds.Assets.BenchDrill, AssetCode = "ACT-0002", Name = "Taladro de banco", Description = "Taladro para trabajos de taller", Brand = "Bosch", Model = "GBM 13-2 RE", SerialNumber = "SN-BOSCH-0002", AcquiredOn = new DateTime(2023, 8, 10), PurchaseCost = 185000m, UsefulLifeMonths = 72, Status = "Operational", UsageHours = 950, UsageHoursAtLastMaintenance = 600, Criticality = "High", CategoryId = SeedIds.AssetCategories.IndustrialEquipment, LocationId = SeedIds.Locations.Workshop, SupplierId = SeedIds.Suppliers.IndustrialMaintenance, CreatedAt = new DateTime(2023, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Asset { Id = SeedIds.Assets.ReceptionDesk, AssetCode = "ACT-0003", Name = "Escritorio de recepción", Description = "Módulo de recepción", Brand = "Oficina CR", Model = "Recepción 180", AcquiredOn = new DateTime(2022, 3, 2), PurchaseCost = 210000m, UsefulLifeMonths = 120, Status = "Operational", UsageHours = 0, UsageHoursAtLastMaintenance = 0, Criticality = "Low", CategoryId = SeedIds.AssetCategories.Furniture, LocationId = SeedIds.Locations.MainBuilding, CreatedAt = new DateTime(2022, 3, 2, 0, 0, 0, DateTimeKind.Utc) });

        modelBuilder.Entity<MaintenanceType>().HasData(
            new MaintenanceType { Id = SeedIds.MaintenanceTypes.Preventive, Name = "Preventivo", Description = "Mantenimiento programado para prevenir fallas", DefaultIntervalDays = 90 },
            new MaintenanceType { Id = SeedIds.MaintenanceTypes.Corrective, Name = "Correctivo", Description = "Reparación de una falla reportada" },
            new MaintenanceType { Id = SeedIds.MaintenanceTypes.Inspection, Name = "Inspección", Description = "Revisión general del estado del activo", DefaultIntervalDays = 180 });

        modelBuilder.Entity<MaintenanceOrder>().HasData(
            new MaintenanceOrder { Id = SeedIds.MaintenanceOrders.LaptopPreventiveMaintenance, WorkOrderCode = "OT-2026-0001", AssetId = SeedIds.Assets.AdministrationLaptop, MaintenanceTypeId = SeedIds.MaintenanceTypes.Preventive, SupplierId = SeedIds.Suppliers.TechnicalServices, Title = "Limpieza y revisión preventiva", Description = "Limpieza interna, revisión de batería y diagnóstico", Reason = "Mantenimiento preventivo según el intervalo de 90 días", Priority = "Medium", Status = "Requested", RequestedAt = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc), ScheduledFor = new DateTime(2026, 10, 15), TotalCost = 25000m, CreatedAt = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc) },
            new MaintenanceOrder { Id = SeedIds.MaintenanceOrders.DrillSafetyInspection, WorkOrderCode = "OT-2026-0002", AssetId = SeedIds.Assets.BenchDrill, MaintenanceTypeId = SeedIds.MaintenanceTypes.Inspection, SupplierId = SeedIds.Suppliers.IndustrialMaintenance, Title = "Inspección de seguridad", Description = "Verificar broca, cableado y fijación de la herramienta", Reason = "Inspección periódica de seguridad del equipo de taller", Priority = "High", Status = "Completed", RequestedAt = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc), ScheduledFor = new DateTime(2026, 8, 20), StartedAt = new DateTime(2026, 8, 20, 8, 0, 0, DateTimeKind.Utc), ClosedAt = new DateTime(2026, 8, 20, 10, 0, 0, DateTimeKind.Utc), TotalCost = 18000m, Findings = "Equipo operativo; se ajustó la base", CreatedAt = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
            new MaintenanceOrder { Id = SeedIds.MaintenanceOrders.DeskStructureReview, WorkOrderCode = "OT-2026-0003", AssetId = SeedIds.Assets.ReceptionDesk, MaintenanceTypeId = SeedIds.MaintenanceTypes.Preventive, Title = "Revisión de estructura", Description = "Ajustar uniones y comprobar estabilidad", Reason = "Se reportan uniones flojas en el módulo de recepción", Priority = "Low", Status = "Requested", RequestedAt = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc), ScheduledFor = new DateTime(2026, 11, 5), CreatedAt = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc) });

        return modelBuilder;
    }
}