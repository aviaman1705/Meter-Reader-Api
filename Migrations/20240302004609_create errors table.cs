using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeterReaderAPI.Migrations
{
    /// <inheritdoc />
    public partial class createerrorstable : Migration
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
                    LogLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThreadId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Notebooks_NotebookId",
                table: "Tracks",
                column: "NotebookId",
                principalTable: "Notebooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
