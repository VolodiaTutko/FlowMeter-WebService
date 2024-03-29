using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class StableVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_consumers_houses_HouseId",
                table: "consumers");

            migrationBuilder.AddForeignKey(
                name: "FK_consumers_houses_HouseId",
                table: "consumers",
                column: "HouseId",
                principalTable: "houses",
                principalColumn: "HouseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_consumers_houses_HouseId",
                table: "consumers");

            migrationBuilder.AddForeignKey(
                name: "FK_consumers_houses_HouseId",
                table: "consumers",
                column: "HouseId",
                principalTable: "houses",
                principalColumn: "HouseId");
        }
    }
}
