using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlowMeter_WebService.Migrations
{
    /// <inheritdoc />
    public partial class FixThreeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accruals_accounts_PersonalAccount",
                table: "accruals");

            migrationBuilder.DropForeignKey(
                name: "FK_consumers_accounts_PersonalAccount",
                table: "consumers");

            migrationBuilder.DropForeignKey(
                name: "FK_payments_accounts_PersonalAccount",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_receipts_accounts_PersonalAccount",
                table: "receipts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_consumers",
                table: "consumers");

            migrationBuilder.DropIndex(
                name: "IX_consumers_PersonalAccount",
                table: "consumers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "ConsumersId",
                table: "consumers");

            migrationBuilder.RenameColumn(
                name: "AccuralID",
                table: "accruals",
                newName: "AccrualID");

            migrationBuilder.AddColumn<byte[]>(
                name: "PDF",
                table: "receipts",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_consumers",
                table: "consumers",
                column: "PersonalAccount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                table: "accounts",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_PersonalAccount",
                table: "accounts",
                column: "PersonalAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_consumers_PersonalAccount",
                table: "accounts",
                column: "PersonalAccount",
                principalTable: "consumers",
                principalColumn: "PersonalAccount",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_accruals_consumers_PersonalAccount",
                table: "accruals",
                column: "PersonalAccount",
                principalTable: "consumers",
                principalColumn: "PersonalAccount",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_consumers_PersonalAccount",
                table: "payments",
                column: "PersonalAccount",
                principalTable: "consumers",
                principalColumn: "PersonalAccount",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_receipts_consumers_PersonalAccount",
                table: "receipts",
                column: "PersonalAccount",
                principalTable: "consumers",
                principalColumn: "PersonalAccount",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_consumers_PersonalAccount",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_accruals_consumers_PersonalAccount",
                table: "accruals");

            migrationBuilder.DropForeignKey(
                name: "FK_payments_consumers_PersonalAccount",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_receipts_consumers_PersonalAccount",
                table: "receipts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_consumers",
                table: "consumers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_PersonalAccount",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "PDF",
                table: "receipts");

            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "accounts");

            migrationBuilder.RenameColumn(
                name: "AccrualID",
                table: "accruals",
                newName: "AccuralID");

            migrationBuilder.AddColumn<int>(
                name: "ConsumersId",
                table: "consumers",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_consumers",
                table: "consumers",
                column: "ConsumersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                table: "accounts",
                column: "PersonalAccount");

            migrationBuilder.CreateIndex(
                name: "IX_consumers_PersonalAccount",
                table: "consumers",
                column: "PersonalAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_accruals_accounts_PersonalAccount",
                table: "accruals",
                column: "PersonalAccount",
                principalTable: "accounts",
                principalColumn: "PersonalAccount",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_consumers_accounts_PersonalAccount",
                table: "consumers",
                column: "PersonalAccount",
                principalTable: "accounts",
                principalColumn: "PersonalAccount",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_accounts_PersonalAccount",
                table: "payments",
                column: "PersonalAccount",
                principalTable: "accounts",
                principalColumn: "PersonalAccount",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_receipts_accounts_PersonalAccount",
                table: "receipts",
                column: "PersonalAccount",
                principalTable: "accounts",
                principalColumn: "PersonalAccount",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
