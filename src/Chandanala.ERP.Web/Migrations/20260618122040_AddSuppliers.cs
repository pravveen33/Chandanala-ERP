using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chandanala.ERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSuppliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CreditDays",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CreditLimit",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "StateCode",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "WhatsAppNumber",
                table: "Suppliers",
                newName: "MobileNumber");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Suppliers",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Suppliers",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "Suppliers",
                newName: "WhatsAppNumber");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Suppliers",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CreditDays",
                table: "Suppliers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                table: "Suppliers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateCode",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
