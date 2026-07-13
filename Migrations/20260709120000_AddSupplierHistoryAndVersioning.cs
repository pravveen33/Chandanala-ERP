using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chandanala.ERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierHistoryAndVersioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Business-level version counter, incremented on every Supplier update.
            migrationBuilder.AddColumn<int>(
                name: "CurrentVersion",
                table: "Suppliers",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            // NOTE: RowVersion is intentionally NOT created here — it is mapped
            // directly onto PostgreSQL's built-in `xmin` system column via
            // HasColumnName("xmin") in OnModelCreating, so there is no real
            // column for EF to add. Every row already has xmin maintained by
            // Postgres itself; this migration only needs the CurrentVersion
            // column and the new SupplierHistories table.

            migrationBuilder.CreateTable(
                name: "SupplierHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    SupplierCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SupplierName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GSTIN = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    DrugLicenseNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContactPerson = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MobileNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pincode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreditDays = table.Column<int>(type: "integer", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierHistories_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierHistories_SupplierId_VersionNumber",
                table: "SupplierHistories",
                columns: new[] { "SupplierId", "VersionNumber" },
                unique: true);

            // Backfill CurrentVersion = 1 for every existing supplier (the
            // default value above only applies to the column definition; this
            // makes sure rows inserted before this migration are consistent).
            migrationBuilder.Sql("UPDATE \"Suppliers\" SET \"CurrentVersion\" = 1 WHERE \"CurrentVersion\" IS NULL OR \"CurrentVersion\" = 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierHistories");

            migrationBuilder.DropColumn(
                name: "CurrentVersion",
                table: "Suppliers");
        }
    }
}
