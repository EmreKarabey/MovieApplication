using System;
using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLikedMovieTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedMovies_Users_UserEntityID",
                table: "LikedMovies");

            migrationBuilder.DropIndex(
                name: "IX_LikedMovies_UserEntityID",
                table: "LikedMovies");

            migrationBuilder.DropColumn(
                name: "UserEntityID",
                table: "LikedMovies");

            // Guid -> int direkt dönüşüm yapılamaz, drop + add gerekiyor
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "LikedMovies");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "LikedMovies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LikedMovies_UserID",
                table: "LikedMovies",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMovies_Users_UserID",
                table: "LikedMovies",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "EntityID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedMovies_Users_UserID",
                table: "LikedMovies");

            migrationBuilder.DropIndex(
                name: "IX_LikedMovies_UserID",
                table: "LikedMovies");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "LikedMovies");

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "LikedMovies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<int>(
                name: "UserEntityID",
                table: "LikedMovies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LikedMovies_UserEntityID",
                table: "LikedMovies",
                column: "UserEntityID");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMovies_Users_UserEntityID",
                table: "LikedMovies",
                column: "UserEntityID",
                principalTable: "Users",
                principalColumn: "EntityID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}