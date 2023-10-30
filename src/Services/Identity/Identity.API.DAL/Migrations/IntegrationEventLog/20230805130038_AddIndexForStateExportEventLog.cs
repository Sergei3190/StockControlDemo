using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.API.DAL.Migrations.IntegrationEventLog
{
    /// <inheritdoc />
    public partial class AddIndexForStateExportEventLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_export_integration_event_log_state",
                schema: "logger",
                table: "export_integration_event_log",
                column: "state");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_export_integration_event_log_state",
                schema: "logger",
                table: "export_integration_event_log");
        }
    }
}
