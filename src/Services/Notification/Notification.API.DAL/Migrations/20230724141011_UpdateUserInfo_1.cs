using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserInfo_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_created",
                schema: "app",
                table: "users_info");

            migrationBuilder.DropColumn(
                name: "_deleted",
                schema: "app",
                table: "users_info");

            migrationBuilder.DropColumn(
                name: "_updated",
                schema: "app",
                table: "users_info");

            migrationBuilder.AddColumn<bool>(
                name: "is_lockout",
                schema: "app",
                table: "users_info",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 11, 4, DateTimeKind.Unspecified).AddTicks(4226), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 852, DateTimeKind.Unspecified).AddTicks(3789), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notification_settings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 11, 6, DateTimeKind.Unspecified).AddTicks(6581), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 854, DateTimeKind.Unspecified).AddTicks(5199), new TimeSpan(0, 4, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_lockout",
                schema: "app",
                table: "users_info");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "_created",
                schema: "app",
                table: "users_info",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 851, DateTimeKind.Unspecified).AddTicks(9720), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "_deleted",
                schema: "app",
                table: "users_info",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "_updated",
                schema: "app",
                table: "users_info",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 852, DateTimeKind.Unspecified).AddTicks(3789), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 11, 4, DateTimeKind.Unspecified).AddTicks(4226), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notification_settings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 854, DateTimeKind.Unspecified).AddTicks(5199), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 11, 6, DateTimeKind.Unspecified).AddTicks(6581), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
