using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixActivitiesMovieIDType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Movies_MovieEntityID",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_MovieEntityID",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "MovieEntityID",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "MovieID",
                table: "Activities");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieID",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MovieID",
                table: "Activities",
                column: "MovieID");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Movies_MovieID",
                table: "Activities",
                column: "MovieID",
                principalTable: "Movies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Movies_MovieID",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_MovieID",
                table: "Activities");

            migrationBuilder.AlterColumn<int>(
                name: "MovieID",
                table: "Activities",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieEntityID",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MovieEntityID",
                table: "Activities",
                column: "MovieEntityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Movies_MovieEntityID",
                table: "Activities",
                column: "MovieEntityID",
                principalTable: "Movies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
