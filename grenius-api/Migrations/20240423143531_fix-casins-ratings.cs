using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace grenius_api.Migrations
{
    /// <inheritdoc />
    public partial class fixcasinsratings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistsRating_artists_ArtistId",
                table: "ArtistsRating");

            migrationBuilder.DropForeignKey(
                name: "FK_SongsRating_songs_SongId",
                table: "SongsRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SongsRating",
                table: "SongsRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistsRating",
                table: "ArtistsRating");

            migrationBuilder.RenameTable(
                name: "SongsRating",
                newName: "songs_rating");

            migrationBuilder.RenameTable(
                name: "ArtistsRating",
                newName: "artists_rating");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "songs_rating",
                newName: "count");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "songs_rating",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SongId",
                table: "songs_rating",
                newName: "song_id");

            migrationBuilder.RenameIndex(
                name: "IX_SongsRating_SongId",
                table: "songs_rating",
                newName: "IX_songs_rating_song_id");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "artists_rating",
                newName: "count");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "artists_rating",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ArtistId",
                table: "artists_rating",
                newName: "artist_id");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistsRating_ArtistId",
                table: "artists_rating",
                newName: "IX_artists_rating_artist_id");

            migrationBuilder.AlterColumn<long>(
                name: "count",
                table: "songs_rating",
                type: "BIGINT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "count",
                table: "artists_rating",
                type: "BIGINT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_songs_rating",
                table: "songs_rating",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_artists_rating",
                table: "artists_rating",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_artists_rating_artists_artist_id",
                table: "artists_rating",
                column: "artist_id",
                principalTable: "artists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_songs_rating_songs_song_id",
                table: "songs_rating",
                column: "song_id",
                principalTable: "songs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_artists_rating_artists_artist_id",
                table: "artists_rating");

            migrationBuilder.DropForeignKey(
                name: "FK_songs_rating_songs_song_id",
                table: "songs_rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_songs_rating",
                table: "songs_rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_artists_rating",
                table: "artists_rating");

            migrationBuilder.RenameTable(
                name: "songs_rating",
                newName: "SongsRating");

            migrationBuilder.RenameTable(
                name: "artists_rating",
                newName: "ArtistsRating");

            migrationBuilder.RenameColumn(
                name: "count",
                table: "SongsRating",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SongsRating",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "song_id",
                table: "SongsRating",
                newName: "SongId");

            migrationBuilder.RenameIndex(
                name: "IX_songs_rating_song_id",
                table: "SongsRating",
                newName: "IX_SongsRating_SongId");

            migrationBuilder.RenameColumn(
                name: "count",
                table: "ArtistsRating",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ArtistsRating",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "artist_id",
                table: "ArtistsRating",
                newName: "ArtistId");

            migrationBuilder.RenameIndex(
                name: "IX_artists_rating_artist_id",
                table: "ArtistsRating",
                newName: "IX_ArtistsRating_ArtistId");

            migrationBuilder.AlterColumn<long>(
                name: "Count",
                table: "SongsRating",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "BIGINT");

            migrationBuilder.AlterColumn<long>(
                name: "Count",
                table: "ArtistsRating",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "BIGINT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongsRating",
                table: "SongsRating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistsRating",
                table: "ArtistsRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistsRating_artists_ArtistId",
                table: "ArtistsRating",
                column: "ArtistId",
                principalTable: "artists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongsRating_songs_SongId",
                table: "SongsRating",
                column: "SongId",
                principalTable: "songs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
