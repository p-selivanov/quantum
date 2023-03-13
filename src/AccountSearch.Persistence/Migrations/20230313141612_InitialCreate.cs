using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quantum.AccountSearch.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "CustomerAccounts",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "citext", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "citext", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "citext", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "citext", maxLength: 20, nullable: true),
                    Balance = table.Column<decimal>(type: "numeric(24,8)", precision: 24, scale: 8, nullable: false),
                    CustomerCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BalanceUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HadSuspension = table.Column<bool>(type: "boolean", nullable: false),
                    FirstDepositTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAccounts", x => new { x.CustomerId, x.Currency });
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_Balance",
                table: "CustomerAccounts",
                column: "Balance");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_BalanceUpdatedAt",
                table: "CustomerAccounts",
                column: "BalanceUpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_Country",
                table: "CustomerAccounts",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_CustomerCreatedAt",
                table: "CustomerAccounts",
                column: "CustomerCreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_EmailAddress",
                table: "CustomerAccounts",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_FirstName",
                table: "CustomerAccounts",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_LastName",
                table: "CustomerAccounts",
                column: "LastName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAccounts");
        }
    }
}
