using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;

namespace Proyecto.Infraestructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AssetCategory> AssetCategories => Set<AssetCategory>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<MaintenanceType> MaintenanceTypes => Set<MaintenanceType>();
    public DbSet<MaintenanceOrder> MaintenanceOrders => Set<MaintenanceOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetCategory>(entity =>
        {
            entity.ToTable("AssetCategories");
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Name).HasMaxLength(80).IsRequired();
            entity.Property(category => category.Description).HasMaxLength(250);
            entity.Property(category => category.IsActive).HasDefaultValue(true);
            entity.HasIndex(category => category.Name).IsUnique();
        });
        modelBuilder.Entity<AssetCategory>().HasData(
            new AssetCategory { Id = 1, Name = "Equipo de cómputo", Description = "Computadoras y periféricos" },
            new AssetCategory { Id = 2, Name = "Mobiliario", Description = "Muebles y estaciones de trabajo" },
            new AssetCategory { Id = 3, Name = "Equipo industrial", Description = "Máquinas y herramientas de producción" });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Locations");
            entity.HasKey(location => location.Id);
            entity.Property(location => location.Name).HasMaxLength(100).IsRequired();
            entity.Property(location => location.Description).HasMaxLength(250);
            entity.Property(location => location.IsActive).HasDefaultValue(true);
            entity.HasIndex(location => location.Name).IsUnique();
        });
        modelBuilder.Entity<Location>().HasData(
            new Location { Id = 1, Name = "Edificio principal", Description = "Sede central" },
            new Location { Id = 2, Name = "Taller", Description = "Área de mantenimiento" },
            new Location { Id = 3, Name = "Bodega", Description = "Almacenamiento de activos" });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Suppliers");
            entity.HasKey(supplier => supplier.Id);
            entity.Property(supplier => supplier.Name).HasMaxLength(120).IsRequired();
            entity.Property(supplier => supplier.TaxId).HasMaxLength(30).IsRequired();
            entity.Property(supplier => supplier.ContactName).HasMaxLength(120);
            entity.Property(supplier => supplier.Phone).HasMaxLength(30);
            entity.Property(supplier => supplier.Email).HasMaxLength(254);
            entity.Property(supplier => supplier.Address).HasMaxLength(250);
            entity.Property(supplier => supplier.IsActive).HasDefaultValue(true);
            entity.HasIndex(supplier => supplier.TaxId).IsUnique();
        });
        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, Name = "Servicios Técnicos CR", TaxId = "3-101-000001", ContactName = "María Rodríguez", Phone = "2200-1000", Email = "servicio@example.com", Address = "San José" },
            new Supplier { Id = 2, Name = "Mantenimiento Industrial del Valle", TaxId = "3-101-000002", ContactName = "Carlos Méndez", Phone = "2200-2000", Email = "taller@example.com", Address = "Heredia" });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("Assets", table =>
            {
                table.HasCheckConstraint("CK_Assets_PurchaseCost", "[PurchaseCost] >= 0");
                table.HasCheckConstraint("CK_Assets_UsefulLifeMonths", "[UsefulLifeMonths] > 0");
                table.HasCheckConstraint("CK_Assets_Status", "[Status] IN ('Active', 'UnderMaintenance', 'Retired')");
            });
            entity.HasKey(asset => asset.Id);
            entity.Property(asset => asset.AssetCode).HasMaxLength(30).IsRequired();
            entity.Property(asset => asset.Name).HasMaxLength(120).IsRequired();
            entity.Property(asset => asset.Description).HasMaxLength(500);
            entity.Property(asset => asset.Brand).HasMaxLength(80);
            entity.Property(asset => asset.Model).HasMaxLength(80);
            entity.Property(asset => asset.SerialNumber).HasMaxLength(100);
            entity.Property(asset => asset.PurchaseCost).HasPrecision(18, 2);
            entity.Property(asset => asset.Status).HasMaxLength(20).HasDefaultValue("Active").IsRequired();
            entity.Property(asset => asset.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(asset => asset.AssetCode).IsUnique();
            entity.HasIndex(asset => asset.SerialNumber).IsUnique().HasFilter("[SerialNumber] IS NOT NULL");
            entity.HasOne(asset => asset.Category).WithMany(category => category.Assets)
                .HasForeignKey(asset => asset.CategoryId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(asset => asset.Location).WithMany(location => location.Assets)
                .HasForeignKey(asset => asset.LocationId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(asset => asset.Supplier).WithMany(supplier => supplier.Assets)
                .HasForeignKey(asset => asset.SupplierId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Asset>().HasData(
            new Asset { Id = 1, AssetCode = "ACT-0001", Name = "Laptop de administración", Description = "Equipo portátil de oficina", Brand = "Dell", Model = "Latitude 5440", SerialNumber = "SN-DELL-0001", AcquiredOn = new DateTime(2024, 1, 15), PurchaseCost = 625000m, UsefulLifeMonths = 48, CategoryId = 1, LocationId = 1, SupplierId = 1, CreatedAt = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Asset { Id = 2, AssetCode = "ACT-0002", Name = "Taladro de banco", Description = "Taladro para trabajos de taller", Brand = "Bosch", Model = "GBM 13-2 RE", SerialNumber = "SN-BOSCH-0002", AcquiredOn = new DateTime(2023, 8, 10), PurchaseCost = 185000m, UsefulLifeMonths = 72, CategoryId = 3, LocationId = 2, SupplierId = 2, CreatedAt = new DateTime(2023, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Asset { Id = 3, AssetCode = "ACT-0003", Name = "Escritorio de recepción", Description = "Módulo de recepción", Brand = "Oficina CR", Model = "Recepción 180", AcquiredOn = new DateTime(2022, 3, 2), PurchaseCost = 210000m, UsefulLifeMonths = 120, CategoryId = 2, LocationId = 1, CreatedAt = new DateTime(2022, 3, 2, 0, 0, 0, DateTimeKind.Utc) });

        modelBuilder.Entity<MaintenanceType>(entity =>
        {
            entity.ToTable("MaintenanceTypes", table =>
                table.HasCheckConstraint("CK_MaintenanceTypes_DefaultIntervalDays", "[DefaultIntervalDays] IS NULL OR [DefaultIntervalDays] > 0"));
            entity.HasKey(type => type.Id);
            entity.Property(type => type.Name).HasMaxLength(80).IsRequired();
            entity.Property(type => type.Description).HasMaxLength(250);
            entity.HasIndex(type => type.Name).IsUnique();
        });
        modelBuilder.Entity<MaintenanceType>().HasData(
            new MaintenanceType { Id = 1, Name = "Preventivo", Description = "Mantenimiento programado para prevenir fallas", DefaultIntervalDays = 90 },
            new MaintenanceType { Id = 2, Name = "Correctivo", Description = "Reparación de una falla reportada" },
            new MaintenanceType { Id = 3, Name = "Inspección", Description = "Revisión general del estado del activo", DefaultIntervalDays = 180 });

        modelBuilder.Entity<MaintenanceOrder>(entity =>
        {
            entity.ToTable("MaintenanceOrders", table =>
            {
                table.HasCheckConstraint("CK_MaintenanceOrders_Priority", "[Priority] IN ('Low', 'Medium', 'High', 'Urgent')");
                table.HasCheckConstraint("CK_MaintenanceOrders_Status", "[Status] IN ('Planned', 'InProgress', 'Completed', 'Cancelled')");
                table.HasCheckConstraint("CK_MaintenanceOrders_Cost", "[Cost] IS NULL OR [Cost] >= 0");
                table.HasCheckConstraint("CK_MaintenanceOrders_DateOrder", "[CompletedAt] IS NULL OR [StartedAt] IS NULL OR [CompletedAt] >= [StartedAt]");
            });
            entity.HasKey(order => order.Id);
            entity.Property(order => order.WorkOrderCode).HasMaxLength(30).IsRequired();
            entity.Property(order => order.Title).HasMaxLength(150).IsRequired();
            entity.Property(order => order.Description).HasMaxLength(1000);
            entity.Property(order => order.Priority).HasMaxLength(16).HasDefaultValue("Medium").IsRequired();
            entity.Property(order => order.Status).HasMaxLength(20).HasDefaultValue("Planned").IsRequired();
            entity.Property(order => order.Cost).HasPrecision(18, 2);
            entity.Property(order => order.Findings).HasMaxLength(2000);
            entity.Property(order => order.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(order => order.WorkOrderCode).IsUnique();
            entity.HasOne(order => order.Asset).WithMany(asset => asset.MaintenanceOrders)
                .HasForeignKey(order => order.AssetId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(order => order.MaintenanceType).WithMany(type => type.MaintenanceOrders)
                .HasForeignKey(order => order.MaintenanceTypeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(order => order.Supplier).WithMany(supplier => supplier.MaintenanceOrders)
                .HasForeignKey(order => order.SupplierId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<MaintenanceOrder>().HasData(
            new MaintenanceOrder { Id = 1, WorkOrderCode = "OT-2026-0001", AssetId = 1, MaintenanceTypeId = 1, SupplierId = 1, Title = "Limpieza y revisión preventiva", Description = "Limpieza interna, revisión de batería y diagnóstico", Priority = "Medium", Status = "Planned", ScheduledFor = new DateTime(2026, 10, 15), Cost = 25000m, CreatedAt = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc) },
            new MaintenanceOrder { Id = 2, WorkOrderCode = "OT-2026-0002", AssetId = 2, MaintenanceTypeId = 3, SupplierId = 2, Title = "Inspección de seguridad", Description = "Verificar broca, cableado y fijación de la herramienta", Priority = "High", Status = "Completed", ScheduledFor = new DateTime(2026, 8, 20), StartedAt = new DateTime(2026, 8, 20, 8, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2026, 8, 20, 10, 0, 0, DateTimeKind.Utc), Cost = 18000m, Findings = "Equipo operativo; se ajustó la base", CreatedAt = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
            new MaintenanceOrder { Id = 3, WorkOrderCode = "OT-2026-0003", AssetId = 3, MaintenanceTypeId = 1, Title = "Revisión de estructura", Description = "Ajustar uniones y comprobar estabilidad", Priority = "Low", Status = "Planned", ScheduledFor = new DateTime(2026, 11, 5), CreatedAt = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc) });
    }
}