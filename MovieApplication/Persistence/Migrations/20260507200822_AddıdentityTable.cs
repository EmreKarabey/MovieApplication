using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddıdentityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationClaims",
                columns: table => new
                {
                    EntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationClaims", x => x.EntityID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    EntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AuthenticatorTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.EntityID);
                });

            migrationBuilder.CreateTable(
                name: "EMailAuthenticators",
                columns: table => new
                {
                    EntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActivationKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerifed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMailAuthenticators", x => x.EntityID);
                    table.ForeignKey(
                        name: "FK_EMailAuthenticators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "EntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OTPAuthenticators",
                columns: table => new
                {
                    EntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SecretKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsVerifed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPAuthenticators", x => x.EntityID);
                    table.ForeignKey(
                        name: "FK_OTPAuthenticators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "EntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    EntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.EntityID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "EntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOperationClaims",
                columns: table => new
                {
                    EntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    OperationClaimID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOperationClaims", x => x.EntityID);
                    table.ForeignKey(
                        name: "FK_UserOperationClaims_OperationClaims_OperationClaimID",
                        column: x => x.OperationClaimID,
                        principalTable: "OperationClaims",
                        principalColumn: "EntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOperationClaims_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "EntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "EntityID", "AuthenticatorTypes", "CreatedAt", "DeletedAt", "EMail", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "Status", "UpdatedAt" },
                values: new object[] { 1, "[]", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@gmail.com", "Emre", "Admin", new byte[] { 138, 151, 151, 7, 117, 164, 142, 88, 59, 255, 221, 143, 171, 164, 225, 161, 30, 182, 115, 231, 206, 177, 111, 30, 170, 13, 181, 13, 212, 86, 122, 140, 138, 248, 176, 250, 42, 19, 36, 55, 157, 26, 217, 94, 37, 10, 226, 235, 124, 63, 172, 64, 15, 45, 64, 183, 201, 78, 233, 206, 80, 157, 230, 89 }, new byte[] { 172, 6, 117, 35, 97, 92, 137, 96, 84, 156, 87, 36, 76, 100, 159, 215, 79, 79, 213, 34, 65, 0, 166, 173, 178, 120, 113, 176, 187, 12, 15, 174, 214, 27, 57, 135, 179, 161, 103, 116, 37, 237, 202, 233, 104, 189, 212, 128, 162, 131, 148, 126, 69, 164, 150, 46, 119, 213, 59, 157, 60, 206, 213, 252, 225, 183, 148, 91, 97, 100, 49, 158, 12, 222, 173, 95, 49, 207, 55, 113, 153, 5, 255, 12, 223, 30, 65, 171, 223, 57, 201, 40, 129, 35, 125, 193, 69, 2, 252, 94, 204, 65, 239, 26, 45, 47, 23, 159, 156, 177, 19, 189, 247, 137, 250, 229, 64, 166, 153, 175, 149, 85, 133, 246, 75, 212, 235, 250 }, true, null });

            migrationBuilder.CreateIndex(
                name: "IX_EMailAuthenticators_UserId",
                table: "EMailAuthenticators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OTPAuthenticators_UserId",
                table: "OTPAuthenticators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_OperationClaimID",
                table: "UserOperationClaims",
                column: "OperationClaimID");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserID",
                table: "UserOperationClaims",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMailAuthenticators");

            migrationBuilder.DropTable(
                name: "OTPAuthenticators");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UserOperationClaims");

            migrationBuilder.DropTable(
                name: "OperationClaims");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
