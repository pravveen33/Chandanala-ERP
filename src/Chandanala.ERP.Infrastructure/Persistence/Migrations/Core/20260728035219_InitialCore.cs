using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chandanala.ERP.Infrastructure.Persistence.Migrations.Core
{
    /// <inheritdoc />
    public partial class InitialCore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AddressLine1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DosageForm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosageForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineKnowledge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BrandName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GenericName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Strength = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DosageForm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Barcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DrugSchedule = table.Column<string>(type: "text", nullable: false),
                    StorageType = table.Column<string>(type: "text", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineKnowledge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicineCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Barcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BrandName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GenericName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    DosageFormId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitOfMeasureId = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageConditionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicineScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    RackId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShelfId = table.Column<Guid>(type: "uuid", nullable: true),
                    MinimumStock = table.Column<int>(type: "integer", nullable: false),
                    MaximumStock = table.Column<int>(type: "integer", nullable: false),
                    ReorderLevel = table.Column<int>(type: "integer", nullable: false),
                    RequiresPrescription = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDiscontinued = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineSchedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackSizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageCondition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageCondition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineBatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicineId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchNumber = table.Column<string>(type: "text", nullable: false),
                    ManufacturingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Mrp = table.Column<decimal>(type: "numeric", nullable: false),
                    QuantityInStock = table.Column<int>(type: "integer", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineBatch_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerCode",
                table: "Customers",
                column: "CustomerCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatch_MedicineId",
                table: "MedicineBatch",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineKnowledge_Code",
                table: "MedicineKnowledge",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_MedicineCode",
                table: "Medicines",
                column: "MedicineCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackSizes_Code",
                table: "PackSizes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "DosageForm");

            migrationBuilder.DropTable(
                name: "Manufacturer");

            migrationBuilder.DropTable(
                name: "MedicineBatch");

            migrationBuilder.DropTable(
                name: "MedicineKnowledge");

            migrationBuilder.DropTable(
                name: "MedicineSchedule");

            migrationBuilder.DropTable(
                name: "PackSizes");

            migrationBuilder.DropTable(
                name: "StorageCondition");

            migrationBuilder.DropTable(
                name: "Medicines");
        }
    }
}
