using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.API.DAL.Migrations.IntegrationEventLog
{
    /// <inheritdoc />
    public partial class InitialIntegrationEventLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "logger");

            migrationBuilder.CreateTable(
                name: "export_integration_event_log",
                schema: "logger",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creation_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    event_type_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    state = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    times_sent = table.Column<int>(type: "int", nullable: false),
                    error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_export_integration_event_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "import_integration_event_log",
                schema: "logger",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creation_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    event_type_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    processing_end_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_import_integration_event_log", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_export_integration_event_log_event_id",
                schema: "logger",
                table: "export_integration_event_log",
                column: "event_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_export_integration_event_log_event_type_name",
                schema: "logger",
                table: "export_integration_event_log",
                column: "event_type_name");

            migrationBuilder.CreateIndex(
                name: "IX_export_integration_event_log_event_type_name_state",
                schema: "logger",
                table: "export_integration_event_log",
                columns: new[] { "event_type_name", "state" });

            migrationBuilder.CreateIndex(
                name: "IX_export_integration_event_log_state",
                schema: "logger",
                table: "export_integration_event_log",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "IX_import_integration_event_log_event_id",
                schema: "logger",
                table: "import_integration_event_log",
                column: "event_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_import_integration_event_log_event_type_name",
                schema: "logger",
                table: "import_integration_event_log",
                column: "event_type_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "export_integration_event_log",
                schema: "logger");

            migrationBuilder.DropTable(
                name: "import_integration_event_log",
                schema: "logger");
        }
    }
}
