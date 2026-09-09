using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNotificationSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings");

            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "NotificationSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_ChannelId",
                table: "NotificationSettings",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSettings_Users_ChannelId",
                table: "NotificationSettings",
                column: "ChannelId",
                principalTable: "Users",
                principalColumn: "EntityID");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "EntityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSettings_Users_ChannelId",
                table: "NotificationSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropIndex(
                name: "IX_NotificationSettings_ChannelId",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "NotificationSettings");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "EntityID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
