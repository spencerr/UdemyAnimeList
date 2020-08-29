using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UdemyAnimeList.Domain.Migrations
{
    public partial class CreatedAtUpdatedAtColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Seasons",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Seasons",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateAired",
                table: "Episodes",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Episodes",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Configuration",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Configuration",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "StartAirDate",
                table: "Animes",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EndAirDate",
                table: "Animes",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Animes",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Animes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Animes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateAired",
                table: "Episodes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartAirDate",
                table: "Animes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndAirDate",
                table: "Animes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }
    }
}
