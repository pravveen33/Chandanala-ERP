using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chandanala.ERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class PurchaseModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrugCategory",
                table: "Medicines",
                newName: "ReorderLevel");

            migrationBuilder.AddColumn<int>(
                name: "CurrentStock",
                table: "Medicines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MRP",
                table: "Medicines",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseRate",
                table: "Medicines",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "PurchaseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseId = table.Column<int>(type: "integer", nullable: false),
                    MedicineId = table.Column<int>(type: "integer", nullable: false),
                    BatchNo = table.Column<string>(type: "text", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PurchaseRate = table.Column<decimal>(type: "numeric", nullable: false),
                    MRP = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    NetAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseItems");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropColumn(
                name: "CurrentStock",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "MRP",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "PurchaseRate",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "ReorderLevel",
                table: "Medicines",
                newName: "DrugCategory");
        }
    }
}
