using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RybalkaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ExpandNoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CloudPct",
                table: "FishingNotes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConditionText",
                table: "FishingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Temp",
                table: "FishingNotes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WindDir",
                table: "FishingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WindKph",
                table: "FishingNotes",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudPct",
                table: "FishingNotes");

            migrationBuilder.DropColumn(
                name: "ConditionText",
                table: "FishingNotes");

            migrationBuilder.DropColumn(
                name: "Temp",
                table: "FishingNotes");

            migrationBuilder.DropColumn(
                name: "WindDir",
                table: "FishingNotes");

            migrationBuilder.DropColumn(
                name: "WindKph",
                table: "FishingNotes");
        }
    }
}
