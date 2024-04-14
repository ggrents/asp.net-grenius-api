using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace grenius_api.Migrations
{
    /// <inheritdoc />
    public partial class lyrics_annotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lyrics",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    song_id = table.Column<int>(type: "int", nullable: false),
                    text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lyrics", x => x.id);
                    table.ForeignKey(
                        name: "FK_lyrics_songs_song_id",
                        column: x => x.song_id,
                        principalTable: "songs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "annotation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    start_symbol = table.Column<int>(type: "int", nullable: false),
                    end_symbol = table.Column<int>(type: "int", nullable: false),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    lyrics_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_annotation", x => x.id);
                    table.ForeignKey(
                        name: "FK_annotation_lyrics_lyrics_id",
                        column: x => x.lyrics_id,
                        principalTable: "lyrics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_annotation_lyrics_id",
                table: "annotation",
                column: "lyrics_id");

            migrationBuilder.CreateIndex(
                name: "IX_lyrics_song_id",
                table: "lyrics",
                column: "song_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "annotation");

            migrationBuilder.DropTable(
                name: "lyrics");
        }
    }
}
