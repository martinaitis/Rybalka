using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RybalkaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_Image_FileName_For_FishingNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "FishingNotes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "FishingNotes");
        }
    }
}
