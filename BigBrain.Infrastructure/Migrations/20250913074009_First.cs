using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigBrain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPhones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    OfficeLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PreferredLanguage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserPrincipalName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserPrincipalName",
                table: "Users",
                column: "UserPrincipalName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
