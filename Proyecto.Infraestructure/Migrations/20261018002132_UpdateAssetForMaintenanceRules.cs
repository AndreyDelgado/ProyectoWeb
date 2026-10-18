using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssetForMaintenanceRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Assets_Status",
                table: "Assets");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Assets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Operational",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Active");

            migrationBuilder.AddColumn<string>(
                name: "Criticality",
                table: "Assets",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "Medium");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Assets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsageHours",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsageHoursAtLastMaintenance",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CodePrefix",
                table: "AssetCategories",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PreventiveThresholdHours",
                table: "AssetCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AssetCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CodePrefix", "PreventiveThresholdHours" },
                values: new object[] { "COMP", 2000 });

            migrationBuilder.UpdateData(
                table: "AssetCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CodePrefix", "PreventiveThresholdHours" },
                values: new object[] { "MOB", 4000 });

            migrationBuilder.UpdateData(
                table: "AssetCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CodePrefix", "PreventiveThresholdHours" },
                values: new object[] { "IND", 500 });

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Criticality", "Image", "Status", "UsageHours", "UsageHoursAtLastMaintenance" },
                values: new object[] { "Medium", null, "Operational", 1800, 0 });

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Criticality", "Image", "Status", "UsageHours", "UsageHoursAtLastMaintenance" },
                values: new object[] { "High", null, "Operational", 950, 600 });

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Criticality", "Image", "Status", "UsageHours", "UsageHoursAtLastMaintenance" },
                values: new object[] { "Low", null, "Operational", 0, 0 });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Assets_Criticality",
                table: "Assets",
                sql: "[Criticality] IN ('High', 'Medium', 'Low')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Assets_Status",
                table: "Assets",
                sql: "[Status] IN ('Operational', 'UnderMaintenance', 'OutOfService', 'Decommissioned')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Assets_UsageHours",
                table: "Assets",
                sql: "[UsageHours] >= 0 AND [UsageHoursAtLastMaintenance] >= 0 AND [UsageHoursAtLastMaintenance] <= [UsageHours]");

            migrationBuilder.CreateIndex(
                name: "IX_AssetCategories_CodePrefix",
                table: "AssetCategories",
                column: "CodePrefix",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_AssetCategories_CodePrefix",
                table: "AssetCategories",
                sql: "LEN([CodePrefix]) BETWEEN 2 AND 5 AND [CodePrefix] NOT LIKE '%[^A-Z]%' COLLATE Latin1_General_BIN");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AssetCategories_PreventiveThresholdHours",
                table: "AssetCategories",
                sql: "[PreventiveThresholdHours] > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Assets_Criticality",
                table: "Assets");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Assets_Status",
                table: "Assets");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Assets_UsageHours",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_AssetCategories_CodePrefix",
                table: "AssetCategories");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AssetCategories_CodePrefix",
                table: "AssetCategories");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AssetCategories_PreventiveThresholdHours",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "Criticality",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "UsageHours",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "UsageHoursAtLastMaintenance",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CodePrefix",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "PreventiveThresholdHours",
                table: "AssetCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Assets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Active",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Operational");

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: "Active");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Assets_Status",
                table: "Assets",
                sql: "[Status] IN ('Active', 'UnderMaintenance', 'Retired')");
        }
    }
}
