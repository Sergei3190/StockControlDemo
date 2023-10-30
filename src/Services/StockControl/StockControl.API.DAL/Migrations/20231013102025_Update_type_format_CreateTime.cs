using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockControl.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Update_type_format_CreateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "write_offs",
                type: "time(0)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "write_offs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 379, DateTimeKind.Unspecified).AddTicks(2407), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 17, DateTimeKind.Unspecified).AddTicks(9243), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "warehouses",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 371, DateTimeKind.Unspecified).AddTicks(406), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 10, DateTimeKind.Unspecified).AddTicks(2084), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "stock_availabilities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 385, DateTimeKind.Unspecified).AddTicks(143), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 22, DateTimeKind.Unspecified).AddTicks(9886), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "receipts",
                type: "time(0)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "receipts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 372, DateTimeKind.Unspecified).AddTicks(2700), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 11, DateTimeKind.Unspecified).AddTicks(3371), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "parties",
                type: "time(0)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "parties",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 383, DateTimeKind.Unspecified).AddTicks(3416), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 21, DateTimeKind.Unspecified).AddTicks(5163), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "organizations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 368, DateTimeKind.Unspecified).AddTicks(5829), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 7, DateTimeKind.Unspecified).AddTicks(9651), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "nomenclatures",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 369, DateTimeKind.Unspecified).AddTicks(8412), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 9, DateTimeKind.Unspecified).AddTicks(1109), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "movings",
                type: "time(0)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "movings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 375, DateTimeKind.Unspecified).AddTicks(5492), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 14, DateTimeKind.Unspecified).AddTicks(5099), new TimeSpan(0, 4, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "write_offs",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "write_offs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 17, DateTimeKind.Unspecified).AddTicks(9243), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 379, DateTimeKind.Unspecified).AddTicks(2407), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "warehouses",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 10, DateTimeKind.Unspecified).AddTicks(2084), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 371, DateTimeKind.Unspecified).AddTicks(406), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "stock_availabilities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 22, DateTimeKind.Unspecified).AddTicks(9886), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 385, DateTimeKind.Unspecified).AddTicks(143), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "receipts",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "receipts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 11, DateTimeKind.Unspecified).AddTicks(3371), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 372, DateTimeKind.Unspecified).AddTicks(2700), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "parties",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "parties",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 21, DateTimeKind.Unspecified).AddTicks(5163), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 383, DateTimeKind.Unspecified).AddTicks(3416), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "organizations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 7, DateTimeKind.Unspecified).AddTicks(9651), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 368, DateTimeKind.Unspecified).AddTicks(5829), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "nomenclatures",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 9, DateTimeKind.Unspecified).AddTicks(1109), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 369, DateTimeKind.Unspecified).AddTicks(8412), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "movings",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "movings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 14, DateTimeKind.Unspecified).AddTicks(5099), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 375, DateTimeKind.Unspecified).AddTicks(5492), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
