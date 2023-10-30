using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateUniqueIndexTypeIdUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_notification_settings_notification_type_id",
                schema: "notice",
                table: "notification_settings");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 28, 16, 37, 13, 958, DateTimeKind.Unspecified).AddTicks(3993), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 17, 57, 21, 364, DateTimeKind.Unspecified).AddTicks(2293), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notification_settings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 28, 16, 37, 13, 960, DateTimeKind.Unspecified).AddTicks(7781), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 17, 57, 21, 366, DateTimeKind.Unspecified).AddTicks(2996), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_notification_settings_notification_type_id_user_id",
                schema: "notice",
                table: "notification_settings",
                columns: new[] { "notification_type_id", "user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_notification_settings_notification_type_id_user_id",
                schema: "notice",
                table: "notification_settings");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 17, 57, 21, 364, DateTimeKind.Unspecified).AddTicks(2293), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 28, 16, 37, 13, 958, DateTimeKind.Unspecified).AddTicks(3993), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "notice",
                table: "notification_settings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 17, 57, 21, 366, DateTimeKind.Unspecified).AddTicks(2996), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 28, 16, 37, 13, 960, DateTimeKind.Unspecified).AddTicks(7781), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_notification_settings_notification_type_id",
                schema: "notice",
                table: "notification_settings",
                column: "notification_type_id");
        }
    }
}
