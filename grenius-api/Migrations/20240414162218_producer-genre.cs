using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace grenius_api.Migrations
{
    /// <inheritdoc />
    public partial class producergenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "songs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "genre_id",
                table: "songs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "producer_id",
                table: "songs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", nullable: false),
                    description = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "producers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", nullable: false),
                    surname = table.Column<string>(type: "varchar(50)", nullable: false),
                    nickname = table.Column<string>(type: "varchar(50)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_songs_genre_id",
                table: "songs",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_songs_producer_id",
                table: "songs",
                column: "producer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_songs_genres_genre_id",
                table: "songs",
                column: "genre_id",
                principalTable: "genres",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_songs_producers_producer_id",
                table: "songs",
                column: "producer_id",
                principalTable: "producers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_songs_genres_genre_id",
                table: "songs");

            migrationBuilder.DropForeignKey(
                name: "FK_songs_producers_producer_id",
                table: "songs");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "producers");

            migrationBuilder.DropIndex(
                name: "IX_songs_genre_id",
                table: "songs");

            migrationBuilder.DropIndex(
                name: "IX_songs_producer_id",
                table: "songs");

            migrationBuilder.DropColumn(
                name: "description",
                table: "songs");

            migrationBuilder.DropColumn(
                name: "genre_id",
                table: "songs");

            migrationBuilder.DropColumn(
                name: "producer_id",
                table: "songs");
        }
    }
}
