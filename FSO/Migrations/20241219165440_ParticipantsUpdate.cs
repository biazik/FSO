using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSO.Migrations
{
    /// <inheritdoc />
    public partial class ParticipantsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId_Id",
                table: "Participants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId_Id",
                table: "Participants",
                column: "UserId_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_UserId_Id",
                table: "Participants",
                column: "UserId_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_UserId_Id",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_UserId_Id",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "UserId_Id",
                table: "Participants");
        }
    }
}
