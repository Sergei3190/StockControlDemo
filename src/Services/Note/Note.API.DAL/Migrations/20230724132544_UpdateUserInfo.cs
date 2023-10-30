using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Note.API.DAL.Migrations
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
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 25, 44, 792, DateTimeKind.Unspecified).AddTicks(9511), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 4, 48, 412, DateTimeKind.Unspecified).AddTicks(2399), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "note",
                table: "notes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 25, 44, 793, DateTimeKind.Unspecified).AddTicks(3267), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 4, 48, 412, DateTimeKind.Unspecified).AddTicks(8821), new TimeSpan(0, 4, 0, 0, 0)));
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
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 4, 48, 412, DateTimeKind.Unspecified).AddTicks(2399), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 25, 44, 792, DateTimeKind.Unspecified).AddTicks(9511), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "note",
                table: "notes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 4, 48, 412, DateTimeKind.Unspecified).AddTicks(8821), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 7, 24, 17, 25, 44, 793, DateTimeKind.Unspecified).AddTicks(3267), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
