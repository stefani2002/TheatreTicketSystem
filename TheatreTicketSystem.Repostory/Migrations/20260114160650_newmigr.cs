using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheatreTicketSystem.Repostory.Migrations
{
    /// <inheritdoc />
    public partial class newmigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Director",
                table: "Plays");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Plays");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Plays");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Plays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Plays",
                columns: new[] { "Id", "Description", "DurationMinutes", "Genre", "Title" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Класична трагедија од Вилијам Шекспир", 180, "Драма", "Хамлет" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Апсурдна драма од Самуел Бекет", 120, "Драма", "Чекајќи го Годо" }
                });

            migrationBuilder.InsertData(
                table: "Shows",
                columns: new[] { "Id", "AvailableSeats", "PlayId", "Price", "ShowTime", "TotalSeats" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), 100, new Guid("11111111-1111-1111-1111-111111111111"), 500m, new DateTime(2026, 1, 15, 17, 6, 50, 522, DateTimeKind.Local).AddTicks(4520), 100 },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 80, new Guid("22222222-2222-2222-2222-222222222222"), 400m, new DateTime(2026, 1, 16, 17, 6, 50, 522, DateTimeKind.Local).AddTicks(4562), 80 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Plays",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Plays",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Plays");

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Plays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Plays",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Plays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
