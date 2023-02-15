using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RybalkaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameNoteToDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "FishingNotes",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "FishingNotes",
                newName: "Note");
        }
    }
}
