using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chandanala.ERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class CompleteSupplierManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "Suppliers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierCode",
                table: "Suppliers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                table: "Suppliers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "GSTIN",
                table: "Suppliers",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "Suppliers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Suppliers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditDays",
                table: "Suppliers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                table: "Suppliers",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DrugLicenseNumber",
                table: "Suppliers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Suppliers"
                SET "DrugLicenseNumber" = NULLIF(trim(concat_ws(' / ', NULLIF("DL20B", ''), NULLIF("DL21B", ''))), '')
                WHERE "DrugLicenseNumber" IS NULL;
                """);

            migrationBuilder.DropColumn(
                name: "DL20B",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "DL21B",
                table: "Suppliers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Suppliers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "Suppliers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Suppliers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_MobileNumber",
                table: "Suppliers",
                column: "MobileNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierCode",
                table: "Suppliers",
                column: "SupplierCode",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierName",
                table: "Suppliers",
                column: "SupplierName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suppliers_MobileNumber",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_SupplierCode",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_SupplierName",
                table: "Suppliers");

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
                name: "DrugLicenseNumber",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Suppliers");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "Suppliers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "SupplierCode",
                table: "Suppliers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GSTIN",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DL20B",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DL21B",
                table: "Suppliers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}


