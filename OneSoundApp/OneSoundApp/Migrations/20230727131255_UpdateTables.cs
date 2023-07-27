using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneSoundApp.Migrations
{
    public partial class UpdateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "SongCount",
                table: "Playlists");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Playlists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_CategoryId",
                table: "Playlists",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Categories_CategoryId",
                table: "Playlists",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Categories_CategoryId",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_CategoryId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Playlists");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SongCount",
                table: "Playlists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
