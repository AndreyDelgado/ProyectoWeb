using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTechniciansAndSpecialties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequiredSpecialtyId",
                table: "MaintenanceTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Technicians",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technicians", x => x.Id);
                    table.CheckConstraint("CK_Technicians_HourlyRate", "[HourlyRate] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "TechnicianSpecialties",
                columns: table => new
                {
                    TechnicianId = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    CertificationLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CertifiedSince = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianSpecialties", x => new { x.TechnicianId, x.SpecialtyId });
                    table.CheckConstraint("CK_TechnicianSpecialties_CertificationLevel", "[CertificationLevel] IN ('Basic', 'Intermediate', 'Advanced')");
                    table.ForeignKey(
                        name: "FK_TechnicianSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicianSpecialties_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "RequiredSpecialtyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "RequiredSpecialtyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "RequiredSpecialtyId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTypes_RequiredSpecialtyId",
                table: "MaintenanceTypes",
                column: "RequiredSpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Name",
                table: "Specialties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_IdentificationNumber",
                table: "Technicians",
                column: "IdentificationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianSpecialties_SpecialtyId",
                table: "TechnicianSpecialties",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTypes_Specialties_RequiredSpecialtyId",
                table: "MaintenanceTypes",
                column: "RequiredSpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTypes_Specialties_RequiredSpecialtyId",
                table: "MaintenanceTypes");

            migrationBuilder.DropTable(
                name: "TechnicianSpecialties");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTypes_RequiredSpecialtyId",
                table: "MaintenanceTypes");

            migrationBuilder.DropColumn(
                name: "RequiredSpecialtyId",
                table: "MaintenanceTypes");
        }
    }
}
