using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigBrain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fifth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCalendarEvents",
                table: "UserCalendarEvents");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserCalendarEvents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCalendarEvents",
                table: "UserCalendarEvents",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendarEvents_UserId",
                table: "UserCalendarEvents",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCalendarEvents",
                table: "UserCalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_UserCalendarEvents_UserId",
                table: "UserCalendarEvents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserCalendarEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCalendarEvents",
                table: "UserCalendarEvents",
                columns: new[] { "UserId", "ICalUId" });
        }
    }
}
