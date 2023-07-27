using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneSoundApp.Migrations
{
    public partial class UpdateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Playlists",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SongCount",
                table: "Playlists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Albums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CategoryId",
                table: "Albums",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Categories_CategoryId",
                table: "Albums",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Categories_CategoryId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_CategoryId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "SongCount",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Albums");
        }
    }
}
