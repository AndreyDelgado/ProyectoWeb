using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSuppliersAndOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_MaintenanceOrders_Cost",
                table: "MaintenanceOrders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MaintenanceOrders_DateOrder",
                table: "MaintenanceOrders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MaintenanceOrders_Status",
                table: "MaintenanceOrders");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "MaintenanceOrders",
                newName: "TotalCost");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "MaintenanceOrders",
                newName: "ClosedAt");

            migrationBuilder.AddColumn<string>(
                name: "GeneralConditions",
                table: "Suppliers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmergency",
                table: "MaintenanceTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MaintenanceOrders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Requested",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Planned");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledFor",
                table: "MaintenanceOrders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "MaintenanceOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedAt",
                table: "MaintenanceOrders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.UpdateData(
                table: "MaintenanceOrders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Reason", "RequestedAt", "Status" },
                values: new object[] { "Mantenimiento preventivo según el intervalo de 90 días", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Requested" });

            migrationBuilder.UpdateData(
                table: "MaintenanceOrders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Reason", "RequestedAt" },
                values: new object[] { "Inspección periódica de seguridad del equipo de taller", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "MaintenanceOrders",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Reason", "RequestedAt", "Status" },
                values: new object[] { "Se reportan uniones flojas en el módulo de recepción", new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Requested" });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                column: "GeneralConditions",
                value: null);

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                column: "GeneralConditions",
                value: null);

            migrationBuilder.AddCheckConstraint(
                name: "CK_MaintenanceOrders_DateOrder",
                table: "MaintenanceOrders",
                sql: "[ClosedAt] IS NULL OR [StartedAt] IS NULL OR [ClosedAt] >= [StartedAt]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MaintenanceOrders_Status",
                table: "MaintenanceOrders",
                sql: "[Status] IN ('Requested', 'Diagnosed', 'Approved', 'InProgress', 'WaitingForParts', 'Completed', 'Rejected', 'Cancelled')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MaintenanceOrders_TotalCost",
                table: "MaintenanceOrders",
                sql: "[TotalCost] IS NULL OR [TotalCost] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_MaintenanceOrders_DateOrder",
                table: "MaintenanceOrders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MaintenanceOrders_Status",
                table: "MaintenanceOrders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MaintenanceOrders_TotalCost",
                table: "MaintenanceOrders");

            migrationBuilder.DropColumn(
                name: "GeneralConditions",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IsEmergency",
                table: "MaintenanceTypes");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "MaintenanceOrders");

            migrationBuilder.DropColumn(
                name: "RequestedAt",
                table: "MaintenanceOrders");

            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "MaintenanceOrders",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "ClosedAt",
                table: "MaintenanceOrders",
                newName: "CompletedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MaintenanceOrders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Planned",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Requested");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledFor",
                table: "MaintenanceOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "MaintenanceOrders",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "Planned");

            migrationBuilder.UpdateData(
                table: "MaintenanceOrders",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: "Planned");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MaintenanceOrders_Cost",
                table: "MaintenanceOrders",
                sql: "[Cost] IS NULL OR [Cost] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MaintenanceOrders_DateOrder",
                table: "MaintenanceOrders",
                sql: "[CompletedAt] IS NULL OR [StartedAt] IS NULL OR [CompletedAt] >= [StartedAt]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MaintenanceOrders_Status",
                table: "MaintenanceOrders",
                sql: "[Status] IN ('Planned', 'InProgress', 'Completed', 'Cancelled')");
        }
    }
}
