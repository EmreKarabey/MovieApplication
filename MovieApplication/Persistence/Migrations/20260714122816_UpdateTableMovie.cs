using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_PublisherId",
                table: "Movies",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_PublisherId",
                table: "Movies",
                column: "PublisherId",
                principalTable: "Users",
                principalColumn: "EntityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_PublisherId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_PublisherId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Movies");
        }
    }
}
