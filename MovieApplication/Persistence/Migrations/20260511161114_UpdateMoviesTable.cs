using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMoviesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_ProducerID",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ProducerID",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ProducerID",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "ProducerName",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProducerName",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "ProducerID",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ProducerID",
                table: "Movies",
                column: "ProducerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_ProducerID",
                table: "Movies",
                column: "ProducerID",
                principalTable: "Users",
                principalColumn: "EntityID");
        }
    }
}
