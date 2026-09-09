using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mgAddTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityID",
                table: "Movies",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Producer",
                table: "Movies",
                newName: "VideoURL");

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MoviesCategoryID",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProducerID",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    MovieID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "EntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikedMovies",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserEntityID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedMovies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LikedMovies_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedMovies_Users_UserEntityID",
                        column: x => x.UserEntityID,
                        principalTable: "Users",
                        principalColumn: "EntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedMovies",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedMovies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SavedMovies_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedMovies_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "EntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoviesCategories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieEntityID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MoviesCategories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesCategories_Movies_MovieEntityID",
                        column: x => x.MovieEntityID,
                        principalTable: "Movies",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MoviesCategories_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EntityID",
                keyValue: 1,
                column: "AccountType",
                value: "admin");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ProducerID",
                table: "Movies",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MovieID",
                table: "Comments",
                column: "MovieID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserID",
                table: "Comments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMovies_MovieID",
                table: "LikedMovies",
                column: "MovieID");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMovies_UserEntityID",
                table: "LikedMovies",
                column: "UserEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesCategories_CategoryID",
                table: "MoviesCategories",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesCategories_MovieEntityID",
                table: "MoviesCategories",
                column: "MovieEntityID",
                unique: true,
                filter: "[MovieEntityID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesCategories_MovieID",
                table: "MoviesCategories",
                column: "MovieID");

            migrationBuilder.CreateIndex(
                name: "IX_SavedMovies_MovieID",
                table: "SavedMovies",
                column: "MovieID");

            migrationBuilder.CreateIndex(
                name: "IX_SavedMovies_UserID",
                table: "SavedMovies",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_ProducerID",
                table: "Movies",
                column: "ProducerID",
                principalTable: "Users",
                principalColumn: "EntityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_ProducerID",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "LikedMovies");

            migrationBuilder.DropTable(
                name: "MoviesCategories");

            migrationBuilder.DropTable(
                name: "SavedMovies");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ProducerID",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MoviesCategoryID",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ProducerID",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Movies",
                newName: "EntityID");

            migrationBuilder.RenameColumn(
                name: "VideoURL",
                table: "Movies",
                newName: "Producer");
        }
    }
}
