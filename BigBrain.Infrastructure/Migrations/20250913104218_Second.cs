using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigBrain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCalendarEvents",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ICalUId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OriginalStartTimeZone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OriginalEndTimeZone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Response = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReminderMinutesBeforeStart = table.Column<int>(type: "int", nullable: false),
                    IsReminderOn = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCalendarEvents", x => new { x.UserId, x.ICalUId });
                    table.ForeignKey(
                        name: "FK_UserCalendarEvents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCalendarEvents");
        }
    }
}
