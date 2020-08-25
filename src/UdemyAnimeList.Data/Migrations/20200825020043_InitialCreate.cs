using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UdemyAnimeList.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AiringSeason = table.Column<int>(nullable: false),
                    Year = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SeasonId = table.Column<Guid>(nullable: true),
                    JapaneseName = table.Column<string>(nullable: true),
                    EnglishName = table.Column<string>(nullable: true),
                    Synopsys = table.Column<string>(nullable: true),
                    Background = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    EpisodeCount = table.Column<int>(nullable: true),
                    StartAirDate = table.Column<DateTime>(nullable: true),
                    EndAirDate = table.Column<DateTime>(nullable: true),
                    BroadcastTime = table.Column<DateTime>(nullable: true),
                    ShowType = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animes_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AnimeId = table.Column<Guid>(nullable: false),
                    JapaneseName = table.Column<string>(nullable: true),
                    EnglishName = table.Column<string>(nullable: true),
                    Number = table.Column<short>(nullable: false),
                    Synopsys = table.Column<string>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: true),
                    DateAired = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animes_SeasonId",
                table: "Animes",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_AnimeId",
                table: "Episodes",
                column: "AnimeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.DropTable(
                name: "Seasons");
        }
    }
}
