using Microsoft.EntityFrameworkCore.Migrations;

namespace Wisieilec.Migrations
{
    public partial class Games_add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfLives",
                table: "Lobbies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RemainingLives = table.Column<int>(nullable: false),
                    UsedLetters = table.Column<string>(nullable: true),
                    WordId = table.Column<int>(nullable: false),
                    LobbyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_LobbyId",
                table: "Games",
                column: "LobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WordId",
                table: "Games",
                column: "WordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropColumn(
                name: "NumberOfLives",
                table: "Lobbies");
        }
    }
}
