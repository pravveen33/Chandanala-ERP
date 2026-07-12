using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chandanala.ERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaAfterRebase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ExpectedRowVersion",
                table: "SupplierChangeRequests",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "xid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "ExpectedRowVersion",
                table: "SupplierChangeRequests",
                type: "xid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
