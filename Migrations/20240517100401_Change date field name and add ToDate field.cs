using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeterReaderAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedatefieldnameandaddToDatefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Tracks",
                newName: "ToDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Tracks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024,6,6, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Tracks");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "Tracks",
                newName: "Date");
        }
    }
}
