using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chandanala.ERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class CreateMedicineTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenericName",
                table: "Medicines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GenericName",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
