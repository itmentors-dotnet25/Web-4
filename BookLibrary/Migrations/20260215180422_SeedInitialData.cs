using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookLibrary.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "BirthYear", "Country", "CreatedAt", "DeathYear", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, null, null, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), null, true, "Агата Кристи", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) },
                    { 2, null, null, null, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), null, true, "Артур Конан Дойл", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) },
                    { 3, null, null, null, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), null, true, "Джоан Роулинг", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) },
                    { 4, null, null, null, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), null, true, "Фёдор Достоевский", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) },
                    { 5, null, null, null, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), null, true, "Лев Толстой", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), null, "Детектив", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) },
                    { 2, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), null, "Фэнтези", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) },
                    { 3, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), null, "Классика", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) },
                    { 4, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), null, "Научная фантастика", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) },
                    { 5, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), null, "Роман", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "CategoryId", "CreatedAt", "Genre", "Isbn", "IsAvailable", "PublicationYear", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), "Детектив", "978-5-17-101234-5", true, 1939, "Десять негритят", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) },
                    { 2, 1, 1, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), "Детектив", "978-5-17-101235-2", true, 1934, "Убийство в Восточном экспрессе", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) },
                    { 3, 3, 2, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), "Фэнтези", "978-5-17-112345-6", true, 1997, "Гарри Поттер и философский камень", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) },
                    { 4, 4, 3, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), "Роман", "978-5-699-12345-6", true, 1866, "Преступление и наказание", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) },
                    { 5, 5, 5, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), "Роман", "978-5-699-23456-7", true, 1869, "Война и мир", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) },
                    { 6, 2, 1, new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), "Детектив", "978-5-699-34567-8", true, 1902, "Шерлок Холмс: Собака Баскервилей", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
