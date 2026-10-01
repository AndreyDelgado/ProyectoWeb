SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF DB_ID(N'MantenimientoActivosDb') IS NULL
    CREATE DATABASE MantenimientoActivosDb;
GO

USE MantenimientoActivosDb;
GO

IF OBJECT_ID(N'dbo.AssetCategories', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AssetCategories
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_AssetCategories PRIMARY KEY,
        Name nvarchar(80) NOT NULL,
        Description nvarchar(250) NULL,
        IsActive bit NOT NULL CONSTRAINT DF_AssetCategories_IsActive DEFAULT (1),
        CONSTRAINT UQ_AssetCategories_Name UNIQUE (Name)
    );
END;

IF OBJECT_ID(N'dbo.Locations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Locations
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Locations PRIMARY KEY,
        Name nvarchar(100) NOT NULL,
        Description nvarchar(250) NULL,
        IsActive bit NOT NULL CONSTRAINT DF_Locations_IsActive DEFAULT (1),
        CONSTRAINT UQ_Locations_Name UNIQUE (Name)
    );
END;

IF OBJECT_ID(N'dbo.Suppliers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Suppliers
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Suppliers PRIMARY KEY,
        Name nvarchar(120) NOT NULL,
        TaxId nvarchar(30) NOT NULL,
        ContactName nvarchar(120) NULL,
        Phone nvarchar(30) NULL,
        Email nvarchar(254) NULL,
        Address nvarchar(250) NULL,
        IsActive bit NOT NULL CONSTRAINT DF_Suppliers_IsActive DEFAULT (1),
        CONSTRAINT UQ_Suppliers_TaxId UNIQUE (TaxId)
    );
END;

IF OBJECT_ID(N'dbo.Assets', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Assets
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Assets PRIMARY KEY,
        AssetCode nvarchar(30) NOT NULL,
        Name nvarchar(120) NOT NULL,
        Description nvarchar(500) NULL,
        Brand nvarchar(80) NULL,
        Model nvarchar(80) NULL,
        SerialNumber nvarchar(100) NULL,
        AcquiredOn datetime2 NOT NULL,
        PurchaseCost decimal(18,2) NOT NULL,
        UsefulLifeMonths int NOT NULL,
        Status nvarchar(20) NOT NULL CONSTRAINT DF_Assets_Status DEFAULT (N'Active'),
        CategoryId int NOT NULL,
        LocationId int NOT NULL,
        SupplierId int NULL,
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_Assets_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Assets_AssetCode UNIQUE (AssetCode),
        CONSTRAINT CK_Assets_PurchaseCost CHECK (PurchaseCost >= 0),
        CONSTRAINT CK_Assets_UsefulLifeMonths CHECK (UsefulLifeMonths > 0),
        CONSTRAINT CK_Assets_Status CHECK (Status IN (N'Active', N'UnderMaintenance', N'Retired')),
        CONSTRAINT FK_Assets_AssetCategories FOREIGN KEY (CategoryId) REFERENCES dbo.AssetCategories(Id),
        CONSTRAINT FK_Assets_Locations FOREIGN KEY (LocationId) REFERENCES dbo.Locations(Id),
        CONSTRAINT FK_Assets_Suppliers FOREIGN KEY (SupplierId) REFERENCES dbo.Suppliers(Id)
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Assets') AND name = N'UX_Assets_SerialNumber')
    CREATE UNIQUE INDEX UX_Assets_SerialNumber ON dbo.Assets(SerialNumber) WHERE SerialNumber IS NOT NULL;

IF OBJECT_ID(N'dbo.MaintenanceTypes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MaintenanceTypes
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_MaintenanceTypes PRIMARY KEY,
        Name nvarchar(80) NOT NULL,
        Description nvarchar(250) NULL,
        DefaultIntervalDays int NULL,
        CONSTRAINT UQ_MaintenanceTypes_Name UNIQUE (Name),
        CONSTRAINT CK_MaintenanceTypes_DefaultIntervalDays CHECK (DefaultIntervalDays IS NULL OR DefaultIntervalDays > 0)
    );
END;

IF OBJECT_ID(N'dbo.MaintenanceOrders', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MaintenanceOrders
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_MaintenanceOrders PRIMARY KEY,
        WorkOrderCode nvarchar(30) NOT NULL,
        AssetId int NOT NULL,
        MaintenanceTypeId int NOT NULL,
        SupplierId int NULL,
        Title nvarchar(150) NOT NULL,
        Description nvarchar(1000) NULL,
        Priority nvarchar(16) NOT NULL CONSTRAINT DF_MaintenanceOrders_Priority DEFAULT (N'Medium'),
        Status nvarchar(20) NOT NULL CONSTRAINT DF_MaintenanceOrders_Status DEFAULT (N'Planned'),
        ScheduledFor datetime2 NOT NULL,
        StartedAt datetime2 NULL,
        CompletedAt datetime2 NULL,
        Cost decimal(18,2) NULL,
        Findings nvarchar(2000) NULL,
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_MaintenanceOrders_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_MaintenanceOrders_WorkOrderCode UNIQUE (WorkOrderCode),
        CONSTRAINT CK_MaintenanceOrders_Priority CHECK (Priority IN (N'Low', N'Medium', N'High', N'Urgent')),
        CONSTRAINT CK_MaintenanceOrders_Status CHECK (Status IN (N'Planned', N'InProgress', N'Completed', N'Cancelled')),
        CONSTRAINT CK_MaintenanceOrders_Cost CHECK (Cost IS NULL OR Cost >= 0),
        CONSTRAINT CK_MaintenanceOrders_DateOrder CHECK (CompletedAt IS NULL OR StartedAt IS NULL OR CompletedAt >= StartedAt),
        CONSTRAINT FK_MaintenanceOrders_Assets FOREIGN KEY (AssetId) REFERENCES dbo.Assets(Id),
        CONSTRAINT FK_MaintenanceOrders_MaintenanceTypes FOREIGN KEY (MaintenanceTypeId) REFERENCES dbo.MaintenanceTypes(Id),
        CONSTRAINT FK_MaintenanceOrders_Suppliers FOREIGN KEY (SupplierId) REFERENCES dbo.Suppliers(Id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.AssetCategories WHERE Name = N'Equipo de cómputo')
    INSERT dbo.AssetCategories (Name, Description, IsActive) VALUES (N'Equipo de cómputo', N'Computadoras y periféricos', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.AssetCategories WHERE Name = N'Mobiliario')
    INSERT dbo.AssetCategories (Name, Description, IsActive) VALUES (N'Mobiliario', N'Muebles y estaciones de trabajo', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.AssetCategories WHERE Name = N'Equipo industrial')
    INSERT dbo.AssetCategories (Name, Description, IsActive) VALUES (N'Equipo industrial', N'Máquinas y herramientas de producción', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Locations WHERE Name = N'Edificio principal')
    INSERT dbo.Locations (Name, Description, IsActive) VALUES (N'Edificio principal', N'Sede central', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Locations WHERE Name = N'Taller')
    INSERT dbo.Locations (Name, Description, IsActive) VALUES (N'Taller', N'Área de mantenimiento', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Locations WHERE Name = N'Bodega')
    INSERT dbo.Locations (Name, Description, IsActive) VALUES (N'Bodega', N'Almacenamiento de activos', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers WHERE TaxId = N'3-101-000001')
    INSERT dbo.Suppliers (Name, TaxId, ContactName, Phone, Email, Address, IsActive) VALUES (N'Servicios Técnicos CR', N'3-101-000001', N'María Rodríguez', N'2200-1000', N'servicio@example.com', N'San José', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers WHERE TaxId = N'3-101-000002')
    INSERT dbo.Suppliers (Name, TaxId, ContactName, Phone, Email, Address, IsActive) VALUES (N'Mantenimiento Industrial del Valle', N'3-101-000002', N'Carlos Méndez', N'2200-2000', N'taller@example.com', N'Heredia', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.MaintenanceTypes WHERE Name = N'Preventivo')
    INSERT dbo.MaintenanceTypes (Name, Description, DefaultIntervalDays) VALUES (N'Preventivo', N'Mantenimiento programado para prevenir fallas', 90);
IF NOT EXISTS (SELECT 1 FROM dbo.MaintenanceTypes WHERE Name = N'Correctivo')
    INSERT dbo.MaintenanceTypes (Name, Description, DefaultIntervalDays) VALUES (N'Correctivo', N'Reparación de una falla reportada', NULL);
IF NOT EXISTS (SELECT 1 FROM dbo.MaintenanceTypes WHERE Name = N'Inspección')
    INSERT dbo.MaintenanceTypes (Name, Description, DefaultIntervalDays) VALUES (N'Inspección', N'Revisión general del estado del activo', 180);

IF NOT EXISTS (SELECT 1 FROM dbo.Assets WHERE AssetCode = N'ACT-0001')
    INSERT dbo.Assets (AssetCode, Name, Description, Brand, Model, SerialNumber, AcquiredOn, PurchaseCost, UsefulLifeMonths, Status, CategoryId, LocationId, SupplierId, CreatedAt)
    VALUES (N'ACT-0001', N'Laptop de administración', N'Equipo portátil de oficina', N'Dell', N'Latitude 5440', N'SN-DELL-0001', '20240115', 625000.00, 48, N'Active',
        (SELECT Id FROM dbo.AssetCategories WHERE Name = N'Equipo de cómputo'), (SELECT Id FROM dbo.Locations WHERE Name = N'Edificio principal'), (SELECT Id FROM dbo.Suppliers WHERE TaxId = N'3-101-000001'), '2024-01-15T00:00:00');
IF NOT EXISTS (SELECT 1 FROM dbo.Assets WHERE AssetCode = N'ACT-0002')
    INSERT dbo.Assets (AssetCode, Name, Description, Brand, Model, SerialNumber, AcquiredOn, PurchaseCost, UsefulLifeMonths, Status, CategoryId, LocationId, SupplierId, CreatedAt)
    VALUES (N'ACT-0002', N'Taladro de banco', N'Taladro para trabajos de taller', N'Bosch', N'GBM 13-2 RE', N'SN-BOSCH-0002', '20230810', 185000.00, 72, N'Active',
        (SELECT Id FROM dbo.AssetCategories WHERE Name = N'Equipo industrial'), (SELECT Id FROM dbo.Locations WHERE Name = N'Taller'), (SELECT Id FROM dbo.Suppliers WHERE TaxId = N'3-101-000002'), '2023-08-10T00:00:00');
IF NOT EXISTS (SELECT 1 FROM dbo.Assets WHERE AssetCode = N'ACT-0003')
    INSERT dbo.Assets (AssetCode, Name, Description, Brand, Model, AcquiredOn, PurchaseCost, UsefulLifeMonths, Status, CategoryId, LocationId, CreatedAt)
    VALUES (N'ACT-0003', N'Escritorio de recepción', N'Módulo de recepción', N'Oficina CR', N'Recepción 180', '20220302', 210000.00, 120, N'Active',
        (SELECT Id FROM dbo.AssetCategories WHERE Name = N'Mobiliario'), (SELECT Id FROM dbo.Locations WHERE Name = N'Edificio principal'), '2022-03-02T00:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.MaintenanceOrders WHERE WorkOrderCode = N'OT-2026-0001')
    INSERT dbo.MaintenanceOrders (WorkOrderCode, AssetId, MaintenanceTypeId, SupplierId, Title, Description, Priority, Status, ScheduledFor, Cost, CreatedAt)
    VALUES (N'OT-2026-0001', (SELECT Id FROM dbo.Assets WHERE AssetCode = N'ACT-0001'), (SELECT Id FROM dbo.MaintenanceTypes WHERE Name = N'Preventivo'),
        (SELECT Id FROM dbo.Suppliers WHERE TaxId = N'3-101-000001'), N'Limpieza y revisión preventiva', N'Limpieza interna, revisión de batería y diagnóstico', N'Medium', N'Planned', '2026-10-15T00:00:00', 25000.00, '2026-09-01T00:00:00');
IF NOT EXISTS (SELECT 1 FROM dbo.MaintenanceOrders WHERE WorkOrderCode = N'OT-2026-0002')
    INSERT dbo.MaintenanceOrders (WorkOrderCode, AssetId, MaintenanceTypeId, SupplierId, Title, Description, Priority, Status, ScheduledFor, StartedAt, CompletedAt, Cost, Findings, CreatedAt)
    VALUES (N'OT-2026-0002', (SELECT Id FROM dbo.Assets WHERE AssetCode = N'ACT-0002'), (SELECT Id FROM dbo.MaintenanceTypes WHERE Name = N'Inspección'),
        (SELECT Id FROM dbo.Suppliers WHERE TaxId = N'3-101-000002'), N'Inspección de seguridad', N'Verificar broca, cableado y fijación de la herramienta', N'High', N'Completed', '2026-08-20T00:00:00', '2026-08-20T08:00:00', '2026-08-20T10:00:00', 18000.00, N'Equipo operativo; se ajustó la base', '2026-08-10T00:00:00');
IF NOT EXISTS (SELECT 1 FROM dbo.MaintenanceOrders WHERE WorkOrderCode = N'OT-2026-0003')
    INSERT dbo.MaintenanceOrders (WorkOrderCode, AssetId, MaintenanceTypeId, Title, Description, Priority, Status, ScheduledFor, CreatedAt)
    VALUES (N'OT-2026-0003', (SELECT Id FROM dbo.Assets WHERE AssetCode = N'ACT-0003'), (SELECT Id FROM dbo.MaintenanceTypes WHERE Name = N'Preventivo'),
        N'Revisión de estructura', N'Ajustar uniones y comprobar estabilidad', N'Low', N'Planned', '2026-11-05T00:00:00', '2026-09-05T00:00:00');
GO