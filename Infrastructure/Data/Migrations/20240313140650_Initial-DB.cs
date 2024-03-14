using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    PersonalAccount = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HotWater = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ColdWater = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Heating = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Electricity = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Gas = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PublicService = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.PersonalAccount);
                });

            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    AdminEmail = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.AdminEmail);
                });

            migrationBuilder.CreateTable(
                name: "counters",
                columns: table => new
                {
                    CountersId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrentIndicator = table.Column<decimal>(type: "numeric", nullable: true),
                    CounterAccount = table.Column<string>(type: "text", nullable: false),
                    TypeOfAccount = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_counters", x => x.CountersId);
                });

            migrationBuilder.CreateTable(
                name: "houses",
                columns: table => new
                {
                    HouseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HouseAddress = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    HeatingAreaOfHouse = table.Column<int>(type: "integer", nullable: true),
                    NumberOfFlat = table.Column<int>(type: "integer", nullable: true),
                    NumberOfResidents = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_houses", x => x.HouseId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    ConsumerEmail = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.ConsumerEmail);
                });

            migrationBuilder.CreateTable(
                name: "accruals",
                columns: table => new
                {
                    AccuralID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonalAccount = table.Column<string>(type: "character varying(10)", nullable: false),
                    Accrued = table.Column<double>(type: "double precision", nullable: false),
                    PreviuosDebit = table.Column<double>(type: "double precision", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accruals", x => x.AccuralID);
                    table.ForeignKey(
                        name: "FK_accruals_accounts_PersonalAccount",
                        column: x => x.PersonalAccount,
                        principalTable: "accounts",
                        principalColumn: "PersonalAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PersonalAccount = table.Column<string>(type: "character varying(10)", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_payments_accounts_PersonalAccount",
                        column: x => x.PersonalAccount,
                        principalTable: "accounts",
                        principalColumn: "PersonalAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receipts",
                columns: table => new
                {
                    ReceiptId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonalAccount = table.Column<string>(type: "character varying(10)", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipts", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK_receipts_accounts_PersonalAccount",
                        column: x => x.PersonalAccount,
                        principalTable: "accounts",
                        principalColumn: "PersonalAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HouseId = table.Column<int>(type: "integer", nullable: true),
                    TypeOfAccount = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_services_houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "houses",
                        principalColumn: "HouseId");
                });

            migrationBuilder.CreateTable(
                name: "consumers",
                columns: table => new
                {
                    ConsumersId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonalAccount = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Flat = table.Column<int>(type: "integer", nullable: false),
                    ConsumerOwner = table.Column<string>(type: "text", nullable: false),
                    HeatingArea = table.Column<int>(type: "integer", nullable: false),
                    HouseId = table.Column<int>(type: "integer", nullable: false),
                    NumberOfPersons = table.Column<int>(type: "integer", nullable: false),
                    ConsumerEmail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consumers", x => x.ConsumersId);
                    table.ForeignKey(
                        name: "FK_consumers_accounts_PersonalAccount",
                        column: x => x.PersonalAccount,
                        principalTable: "accounts",
                        principalColumn: "PersonalAccount",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_consumers_houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "houses",
                        principalColumn: "HouseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_consumers_users_ConsumerEmail",
                        column: x => x.ConsumerEmail,
                        principalTable: "users",
                        principalColumn: "ConsumerEmail",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accruals_PersonalAccount",
                table: "accruals",
                column: "PersonalAccount");

            migrationBuilder.CreateIndex(
                name: "IX_consumers_ConsumerEmail",
                table: "consumers",
                column: "ConsumerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_consumers_HouseId",
                table: "consumers",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_consumers_PersonalAccount",
                table: "consumers",
                column: "PersonalAccount");

            migrationBuilder.CreateIndex(
                name: "IX_payments_PersonalAccount",
                table: "payments",
                column: "PersonalAccount");

            migrationBuilder.CreateIndex(
                name: "IX_receipts_PersonalAccount",
                table: "receipts",
                column: "PersonalAccount");

            migrationBuilder.CreateIndex(
                name: "IX_services_HouseId",
                table: "services",
                column: "HouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accruals");

            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "consumers");

            migrationBuilder.DropTable(
                name: "counters");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "receipts");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "houses");
        }
    }
}
