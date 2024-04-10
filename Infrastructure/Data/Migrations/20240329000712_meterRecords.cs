using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class meterRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentIndicator",
                table: "meters");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "meters");

            migrationBuilder.RenameColumn(
                name: "CountersId",
                table: "meters",
                newName: "MeterId");

            migrationBuilder.CreateTable(
                name: "meterRecords",
                columns: table => new
                {
                    MeterRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrentIndicator = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MeterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meterRecords", x => x.MeterRecordId);
                    table.ForeignKey(
                        name: "FK_meterRecords_meters_MeterId",
                        column: x => x.MeterId,
                        principalTable: "meters",
                        principalColumn: "MeterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_meterRecords_MeterId",
                table: "meterRecords",
                column: "MeterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "meterRecords");

            migrationBuilder.RenameColumn(
                name: "MeterId",
                table: "meters",
                newName: "CountersId");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentIndicator",
                table: "meters",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "meters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
