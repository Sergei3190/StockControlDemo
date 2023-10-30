using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_deleted_date",
                schema: "app",
                table: "users_info",
                newName: "_deleted");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "app",
                table: "users_info",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 851, DateTimeKind.Unspecified).AddTicks(9720), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 967, DateTimeKind.Unspecified).AddTicks(5158), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 852, DateTimeKind.Unspecified).AddTicks(3789), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 967, DateTimeKind.Unspecified).AddTicks(8810), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notification_settings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 854, DateTimeKind.Unspecified).AddTicks(5199), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 969, DateTimeKind.Unspecified).AddTicks(9626), new TimeSpan(0, 4, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_deleted",
                schema: "app",
                table: "users_info",
                newName: "_deleted_date");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "app",
                table: "users_info",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 967, DateTimeKind.Unspecified).AddTicks(5158), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 851, DateTimeKind.Unspecified).AddTicks(9720), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 967, DateTimeKind.Unspecified).AddTicks(8810), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 852, DateTimeKind.Unspecified).AddTicks(3789), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notification_settings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 969, DateTimeKind.Unspecified).AddTicks(9626), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 26, 50, 854, DateTimeKind.Unspecified).AddTicks(5199), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
