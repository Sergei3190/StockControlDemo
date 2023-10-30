using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockControl.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_parties_types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "write_offs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 537, DateTimeKind.Unspecified).AddTicks(1419), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 379, DateTimeKind.Unspecified).AddTicks(2407), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "warehouses",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 528, DateTimeKind.Unspecified).AddTicks(4860), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 371, DateTimeKind.Unspecified).AddTicks(406), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "stock_availabilities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 542, DateTimeKind.Unspecified).AddTicks(5876), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 385, DateTimeKind.Unspecified).AddTicks(143), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "receipts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 529, DateTimeKind.Unspecified).AddTicks(7688), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 372, DateTimeKind.Unspecified).AddTicks(2700), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "parties",
                type: "time(0)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                schema: "stock",
                table: "parties",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "parties",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 541, DateTimeKind.Unspecified).AddTicks(272), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 383, DateTimeKind.Unspecified).AddTicks(3416), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "organizations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 526, DateTimeKind.Unspecified).AddTicks(140), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 368, DateTimeKind.Unspecified).AddTicks(5829), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "nomenclatures",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 527, DateTimeKind.Unspecified).AddTicks(2694), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 369, DateTimeKind.Unspecified).AddTicks(8412), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "movings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 533, DateTimeKind.Unspecified).AddTicks(3630), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 375, DateTimeKind.Unspecified).AddTicks(5492), new TimeSpan(0, 4, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "write_offs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 379, DateTimeKind.Unspecified).AddTicks(2407), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 537, DateTimeKind.Unspecified).AddTicks(1419), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "warehouses",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 371, DateTimeKind.Unspecified).AddTicks(406), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 528, DateTimeKind.Unspecified).AddTicks(4860), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "stock_availabilities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 385, DateTimeKind.Unspecified).AddTicks(143), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 542, DateTimeKind.Unspecified).AddTicks(5876), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "receipts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 372, DateTimeKind.Unspecified).AddTicks(2700), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 529, DateTimeKind.Unspecified).AddTicks(7688), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "create_time",
                schema: "stock",
                table: "parties",
                type: "time(0)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_date",
                schema: "stock",
                table: "parties",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "parties",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 383, DateTimeKind.Unspecified).AddTicks(3416), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 541, DateTimeKind.Unspecified).AddTicks(272), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "organizations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 368, DateTimeKind.Unspecified).AddTicks(5829), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 526, DateTimeKind.Unspecified).AddTicks(140), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "nomenclatures",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 369, DateTimeKind.Unspecified).AddTicks(8412), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 527, DateTimeKind.Unspecified).AddTicks(2694), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "movings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 14, 20, 25, 375, DateTimeKind.Unspecified).AddTicks(5492), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 533, DateTimeKind.Unspecified).AddTicks(3630), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
