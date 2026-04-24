using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheatreTicketSystem.Repostory.Migrations
{
    /// <inheritdoc />
    public partial class m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "ShowTime",
                value: new DateTime(2026, 1, 16, 17, 23, 33, 332, DateTimeKind.Local).AddTicks(3831));

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "ShowTime",
                value: new DateTime(2026, 1, 17, 17, 23, 33, 332, DateTimeKind.Local).AddTicks(3889));
        }
    }
}
