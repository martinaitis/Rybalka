using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RybalkaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FishingNotes_User_OneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "FishingNotes");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "FishingNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FishingNotes_UserId",
                table: "FishingNotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FishingNotes_Users_UserId",
                table: "FishingNotes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FishingNotes_Users_UserId",
                table: "FishingNotes");

            migrationBuilder.DropIndex(
                name: "IX_FishingNotes_UserId",
                table: "FishingNotes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FishingNotes");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "FishingNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
