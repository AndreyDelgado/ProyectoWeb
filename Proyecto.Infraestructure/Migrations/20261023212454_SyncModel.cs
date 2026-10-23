using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssetCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Computadoras y periféricos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssetCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Computadoras y periféricos 1234");
        }
    }
}
