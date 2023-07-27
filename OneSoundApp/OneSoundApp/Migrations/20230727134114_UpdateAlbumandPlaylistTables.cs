using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneSoundApp.Migrations
{
    public partial class UpdateAlbumandPlaylistTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SongCount",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "SongCount",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "SongCount",
                table: "Albums");
        }
    }
}
