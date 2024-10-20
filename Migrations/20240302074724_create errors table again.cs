using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeterReaderAPI.Migrations
{
    /// <inheritdoc />
    public partial class createerrorstableagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Notebooks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Values = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Notebooks_NotebookId",
                table: "Tracks");

            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_NotebookId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "NotebookId",
                table: "Tracks");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Notebooks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "Notebooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
