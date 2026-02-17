using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8850), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8851) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8854), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8854) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8856), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8857) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8858), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8858) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8860), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(8860) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9251), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9252) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9257), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9258) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9261), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9261) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9264), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9264) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9266), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9267) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9269), "Шерлок Холмс: Сокращенное издание", new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9269) });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "CategoryId", "CreatedAt", "Genre", "Isbn", "IsAvailable", "PublicationYear", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 7, 4, 4, new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9272), "Антиутопия", "978-5-17-999999-9", true, 1949, "Идиот", new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9272) },
                    { 8, 4, 4, new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9274), "Фантастика", "978-5-17-888888-8", true, 1965, "Бесы", new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9274) }
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9131), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9131) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9137), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9137) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9139), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9140) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9141), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9142) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9143), new DateTime(2026, 2, 17, 7, 46, 1, 719, DateTimeKind.Utc).AddTicks(9143) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1638) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853), "Шерлок Холмс: Собака Баскервилей", new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783), new DateTime(2026, 2, 15, 18, 4, 22, 24, DateTimeKind.Utc).AddTicks(1783) });
        }
    }
}
