using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSO.Migrations
{
    /// <inheritdoc />
    public partial class DeletingExcessiveCollumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_AspNetUsers_UserId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_UserId1",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_UserId1",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Event_UserId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Event");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Participants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Event",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId1",
                table: "Participants",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Event_UserId",
                table: "Event",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_AspNetUsers_UserId",
                table: "Event",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_UserId1",
                table: "Participants",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
