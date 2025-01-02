using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSO.Migrations
{
    /// <inheritdoc />
    public partial class LatLonUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "lon",
                table: "Location",
                type: "decimal(18,15)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "lat",
                table: "Location",
                type: "decimal(18,15)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "lon",
                table: "Location",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,15)");

            migrationBuilder.AlterColumn<decimal>(
                name: "lat",
                table: "Location",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,15)");
        }
    }
}
