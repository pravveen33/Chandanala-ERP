using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chandanala.ERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class ImplementPurchaseManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "StockTransactions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "BatchNo",
                table: "PurchaseItems",
                newName: "BatchNumber");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "PurchaseItems",
                newName: "LineTotal");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "Purchases",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "Purchases",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Purchases",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Purchases",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<decimal>(
                name: "GSTAmount",
                table: "Purchases",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrandTotal",
                table: "Purchases",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherCharges",
                table: "Purchases",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Purchases",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseNumber",
                table: "Purchases",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RoundOff",
                table: "Purchases",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Purchases",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Completed");

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Purchases",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Purchases",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseRate",
                table: "PurchaseItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "MRP",
                table: "PurchaseItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "BatchNumber",
                table: "PurchaseItems",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "PurchaseItems",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FreeQuantity",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "GSTPercentage",
                table: "PurchaseItems",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "LineTotal",
                table: "PurchaseItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<DateTime>(
                name: "ManufactureDate",
                table: "PurchaseItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "PurchaseItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql("""
                UPDATE "Purchases"
                SET
                    "PurchaseNumber" = 'GRN-LEGACY-' || lpad("Id"::text, 6, '0'),
                    "PurchaseDate" = "InvoiceDate",
                    "CreatedAt" = CURRENT_TIMESTAMP,
                    "Subtotal" = "TotalAmount",
                    "GSTAmount" = "TaxAmount",
                    "GrandTotal" = "NetAmount",
                    "Status" = 'Completed';
                """);

            migrationBuilder.Sql("""
                UPDATE "PurchaseItems"
                SET "SellingPrice" = "MRP"
                WHERE "SellingPrice" = 0;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseNumber",
                table: "Purchases",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Purchases");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseDate",
                table: "Purchases",
                column: "PurchaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseNumber_Unique",
                table: "Purchases",
                column: "PurchaseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SupplierId_InvoiceNumber_Unique",
                table: "Purchases",
                columns: new[] { "SupplierId", "InvoiceNumber" },
                unique: true,
                filter: "\"Status\" <> 'Deleted'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Purchases_PurchaseDate",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PurchaseNumber_Unique",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_SupplierId_InvoiceNumber_Unique",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "GSTAmount",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "OtherCharges",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PurchaseNumber",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "RoundOff",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "BatchNumber",
                table: "PurchaseItems",
                newName: "BatchNo");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "FreeQuantity",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "GSTPercentage",
                table: "PurchaseItems");

            migrationBuilder.RenameColumn(
                name: "LineTotal",
                table: "PurchaseItems",
                newName: "Amount");

            migrationBuilder.DropColumn(
                name: "ManufactureDate",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "PurchaseItems");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "Purchases",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "Purchases",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Purchases",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAmount",
                table: "Purchases",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "Purchases",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Purchases",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseRate",
                table: "PurchaseItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "MRP",
                table: "PurchaseItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "PurchaseItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "BatchNo",
                table: "PurchaseItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
