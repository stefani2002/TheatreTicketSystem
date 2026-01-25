using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheatreTicketSystem.Repostory.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentCardsAndTicketCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PaymentCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardHolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpMonth = table.Column<int>(type: "int", nullable: false),
                    ExpYear = table.Column<int>(type: "int", nullable: false),
                    Cvv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCards", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PaymentCards",
                columns: new[] { "Id", "Balance", "CardHolderName", "CardNumber", "Cvv", "ExpMonth", "ExpYear", "IsActive" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 50000m, "Test User", "4242424242424242", "123", 12, 2030, true });

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "ShowTime",
                value: new DateTime(2026, 1, 16, 18, 4, 7, 370, DateTimeKind.Local).AddTicks(2976));

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "ShowTime",
                value: new DateTime(2026, 1, 17, 18, 4, 7, 370, DateTimeKind.Local).AddTicks(3017));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentCards");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Tickets");

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "ShowTime",
                value: new DateTime(2026, 1, 16, 17, 41, 14, 130, DateTimeKind.Local).AddTicks(9361));

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "ShowTime",
                value: new DateTime(2026, 1, 17, 17, 41, 14, 130, DateTimeKind.Local).AddTicks(9404));
        }
    }
}
