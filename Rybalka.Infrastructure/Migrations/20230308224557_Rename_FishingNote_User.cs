using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RybalkaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Rename_FishingNote_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "FishingNotes",
                newName: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "FishingNotes",
                newName: "User");
        }
    }
}
