using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RybalkaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDurationToEndTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "FishingNotes");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "FishingNotes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "FishingNotes");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "FishingNotes",
                type: "time",
                nullable: true);
        }
    }
}
