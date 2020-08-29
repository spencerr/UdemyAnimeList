using Microsoft.EntityFrameworkCore.Migrations;

namespace UdemyAnimeList.Domain.Migrations
{
    public partial class ModifyTVRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Animes");

            migrationBuilder.AddColumn<int>(
                name: "TVRating",
                table: "Animes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TVRating",
                table: "Animes");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Animes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
