using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.API.DAL.Migrations.IntegrationEventLog
{
    /// <inheritdoc />
    public partial class DeleteReasonIntegrationEventLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_dlx_reason",
                schema: "logger",
                table: "export_integration_event_log");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "last_dlx_reason",
                schema: "logger",
                table: "export_integration_event_log",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
