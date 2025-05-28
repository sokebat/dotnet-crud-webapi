using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace videogameapi.Migrations
{
    /// <inheritdoc />
    public partial class seedingdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "Id", "Developer", "Platform", "Publisher", "Title" },
                values: new object[,]
                {
                    { 1, "Nintendo EPD", "Nintendo Switch", "Nintendo", "The Legend of Zelda: Breath of the Wild" },
                    { 2, "Santa Monica Studio", "PlayStation 4", "Sony Interactive Entertainment", "God of War" },
                    { 3, "Mojang Studios", "Multi-platform", "Mojang Studios", "Minecraft" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
