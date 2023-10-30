using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockControl.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                schema: "stock",
                table: "write_offs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "write_offs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 466, DateTimeKind.Unspecified).AddTicks(6335), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 537, DateTimeKind.Unspecified).AddTicks(1419), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "warehouses",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 458, DateTimeKind.Unspecified).AddTicks(2342), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 528, DateTimeKind.Unspecified).AddTicks(4860), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                schema: "stock",
                table: "stock_availabilities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "stock_availabilities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 472, DateTimeKind.Unspecified).AddTicks(2594), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 542, DateTimeKind.Unspecified).AddTicks(5876), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                schema: "stock",
                table: "receipts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "receipts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 459, DateTimeKind.Unspecified).AddTicks(5337), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 529, DateTimeKind.Unspecified).AddTicks(7688), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "parties",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 470, DateTimeKind.Unspecified).AddTicks(6643), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 541, DateTimeKind.Unspecified).AddTicks(272), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "organizations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 455, DateTimeKind.Unspecified).AddTicks(7554), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 526, DateTimeKind.Unspecified).AddTicks(140), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "nomenclatures",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 456, DateTimeKind.Unspecified).AddTicks(9988), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 527, DateTimeKind.Unspecified).AddTicks(2694), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                schema: "stock",
                table: "movings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "movings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 462, DateTimeKind.Unspecified).AddTicks(8132), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 533, DateTimeKind.Unspecified).AddTicks(3630), new TimeSpan(0, 4, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                schema: "stock",
                table: "write_offs",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "write_offs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 537, DateTimeKind.Unspecified).AddTicks(1419), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 466, DateTimeKind.Unspecified).AddTicks(6335), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "warehouses",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 528, DateTimeKind.Unspecified).AddTicks(4860), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 458, DateTimeKind.Unspecified).AddTicks(2342), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                schema: "stock",
                table: "stock_availabilities",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "stock_availabilities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 542, DateTimeKind.Unspecified).AddTicks(5876), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 472, DateTimeKind.Unspecified).AddTicks(2594), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                schema: "stock",
                table: "receipts",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "receipts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 529, DateTimeKind.Unspecified).AddTicks(7688), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 459, DateTimeKind.Unspecified).AddTicks(5337), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "parties",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 541, DateTimeKind.Unspecified).AddTicks(272), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 470, DateTimeKind.Unspecified).AddTicks(6643), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "organizations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 526, DateTimeKind.Unspecified).AddTicks(140), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 455, DateTimeKind.Unspecified).AddTicks(7554), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "nomenclatures",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 527, DateTimeKind.Unspecified).AddTicks(2694), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 456, DateTimeKind.Unspecified).AddTicks(9988), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                schema: "stock",
                table: "movings",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "_created",
                schema: "stock",
                table: "movings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 19, 19, 11, 23, 533, DateTimeKind.Unspecified).AddTicks(3630), new TimeSpan(0, 4, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 24, 19, 14, 40, 462, DateTimeKind.Unspecified).AddTicks(8132), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
