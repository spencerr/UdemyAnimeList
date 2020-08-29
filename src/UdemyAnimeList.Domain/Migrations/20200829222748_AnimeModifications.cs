using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UdemyAnimeList.Domain.Migrations
{
    public partial class AnimeModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Animes");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "BroadcastTime",
                table: "Animes",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BroadcastTime",
                table: "Animes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Animes",
                type: "text",
                nullable: true);
        }
    }
}
