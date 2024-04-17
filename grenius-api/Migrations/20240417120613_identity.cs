using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace grenius_api.Migrations
{
    /// <inheritdoc />
    public partial class identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_created_id",
                table: "lyrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_created_id",
                table: "annotation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lyrics_user_created_id",
                table: "lyrics",
                column: "user_created_id");

            migrationBuilder.CreateIndex(
                name: "IX_annotation_user_created_id",
                table: "annotation",
                column: "user_created_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_roles_role_id",
                table: "users_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_roles_user_id",
                table: "users_roles",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_annotation_users_user_created_id",
                table: "annotation",
                column: "user_created_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lyrics_users_user_created_id",
                table: "lyrics",
                column: "user_created_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_annotation_users_user_created_id",
                table: "annotation");

            migrationBuilder.DropForeignKey(
                name: "FK_lyrics_users_user_created_id",
                table: "lyrics");

            migrationBuilder.DropTable(
                name: "users_roles");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "IX_lyrics_user_created_id",
                table: "lyrics");

            migrationBuilder.DropIndex(
                name: "IX_annotation_user_created_id",
                table: "annotation");

            migrationBuilder.DropColumn(
                name: "user_created_id",
                table: "lyrics");

            migrationBuilder.DropColumn(
                name: "user_created_id",
                table: "annotation");
        }
    }
}
