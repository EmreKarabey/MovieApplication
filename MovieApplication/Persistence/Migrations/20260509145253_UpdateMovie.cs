using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviesCategories_Movies_MovieEntityID",
                table: "MoviesCategories");

            migrationBuilder.DropIndex(
                name: "IX_MoviesCategories_MovieEntityID",
                table: "MoviesCategories");

            migrationBuilder.DropColumn(
                name: "MovieEntityID",
                table: "MoviesCategories");

            migrationBuilder.DropColumn(
                name: "MoviesCategoryID",
                table: "Movies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MovieEntityID",
                table: "MoviesCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MoviesCategoryID",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MoviesCategories_MovieEntityID",
                table: "MoviesCategories",
                column: "MovieEntityID",
                unique: true,
                filter: "[MovieEntityID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesCategories_Movies_MovieEntityID",
                table: "MoviesCategories",
                column: "MovieEntityID",
                principalTable: "Movies",
                principalColumn: "ID");
        }
    }
}
