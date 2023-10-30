using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Note.API.DAL.Migrations
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
                schema: "note",
                table: "notes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 51, 674, DateTimeKind.Unspecified).AddTicks(1289), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 25, 44, 793, DateTimeKind.Unspecified).AddTicks(3267), new TimeSpan(0, 4, 0, 0, 0)));
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
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 25, 44, 792, DateTimeKind.Unspecified).AddTicks(9511), new TimeSpan(0, 4, 0, 0, 0)));

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
                schema: "note",
                table: "notes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 25, 44, 793, DateTimeKind.Unspecified).AddTicks(3267), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 18, 10, 51, 674, DateTimeKind.Unspecified).AddTicks(1289), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
