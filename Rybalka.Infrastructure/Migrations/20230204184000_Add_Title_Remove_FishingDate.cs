using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RybalkaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleRemoveFishingDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FishingDate",
                table: "FishingNotes");

            migrationBuilder.AlterColumn<decimal>(
                name: "FishCount",
                table: "FishingNotes",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FishingNotes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "FishingNotes");

            migrationBuilder.AlterColumn<decimal>(
                name: "FishCount",
                table: "FishingNotes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FishingDate",
                table: "FishingNotes",
                type: "datetime2",
                nullable: true);
        }
    }
}
