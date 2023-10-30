using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteNotificationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 17, 57, 21, 364, DateTimeKind.Unspecified).AddTicks(2293), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 11, 4, DateTimeKind.Unspecified).AddTicks(4226), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notification_settings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 17, 57, 21, 366, DateTimeKind.Unspecified).AddTicks(2996), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 11, 6, DateTimeKind.Unspecified).AddTicks(6581), new TimeSpan(0, 4, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 11, 4, DateTimeKind.Unspecified).AddTicks(4226), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 17, 57, 21, 364, DateTimeKind.Unspecified).AddTicks(2293), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notification_settings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 11, 6, DateTimeKind.Unspecified).AddTicks(6581), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 17, 57, 21, 366, DateTimeKind.Unspecified).AddTicks(2996), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
