using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chandanala.ERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicineMasterFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduleDrug",
                table: "Medicines",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "PurchaseRate",
                table: "Medicines",
                newName: "SellingPrice");

            migrationBuilder.RenameColumn(
                name: "CurrentStock",
                table: "Medicines",
                newName: "MinimumStock");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Medicines",
                newName: "ShelfNumber");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Medicines",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Medicines",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastValidatedOn",
                table: "Medicines",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MaximumStock",
                table: "Medicines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MedicineCode",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PackSize",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PackType",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Medicines",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "RackNumber",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ScheduleType",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Medicines",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Action = table.Column<string>(type: "text", nullable: false),
                    EntityName = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "LastValidatedOn",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "MaximumStock",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "MedicineCode",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "PackSize",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "PackType",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "RackNumber",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ScheduleType",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "Medicines",
                newName: "ScheduleDrug");

            migrationBuilder.RenameColumn(
                name: "ShelfNumber",
                table: "Medicines",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "Medicines",
                newName: "PurchaseRate");

            migrationBuilder.RenameColumn(
                name: "MinimumStock",
                table: "Medicines",
                newName: "CurrentStock");
        }
    }
}
