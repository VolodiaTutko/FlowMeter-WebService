using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class deleteForeignKeyEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_consumers_users_ConsumerEmail",
                table: "consumers");

            migrationBuilder.DropIndex(
                name: "IX_consumers_ConsumerEmail",
                table: "consumers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_consumers_ConsumerEmail",
                table: "consumers",
                column: "ConsumerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_consumers_users_ConsumerEmail",
                table: "consumers",
                column: "ConsumerEmail",
                principalTable: "users",
                principalColumn: "ConsumerEmail",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
