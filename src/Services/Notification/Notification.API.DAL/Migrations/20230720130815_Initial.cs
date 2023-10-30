using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notice");

            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "notification_types",
                schema: "notice",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mnemo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sources",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mnemo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users_info",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    source_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 967, DateTimeKind.Unspecified).AddTicks(5158), new TimeSpan(0, 4, 0, 0, 0))),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_info_sources_source_id",
                        column: x => x.source_id,
                        principalSchema: "app",
                        principalTable: "sources",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "notification_settings",
                schema: "notice",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    notification_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    enable = table.Column<bool>(type: "bit", nullable: false),
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 969, DateTimeKind.Unspecified).AddTicks(9626), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_settings_notification_types_notification_type_id",
                        column: x => x.notification_type_id,
                        principalSchema: "notice",
                        principalTable: "notification_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notification_settings_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notification_settings_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notification_settings_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notification_settings_users_info_user_id",
                        column: x => x.user_id,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "notice",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    content = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    notification_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_send = table.Column<bool>(type: "bit", nullable: false),
                    send_date = table.Column<DateTime>(type: "date", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 8, 14, 967, DateTimeKind.Unspecified).AddTicks(8810), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifications_notification_types_notification_type_id",
                        column: x => x.notification_type_id,
                        principalSchema: "notice",
                        principalTable: "notification_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notifications_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notifications_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notifications_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notifications_users_info_user_id",
                        column: x => x.user_id,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_notification_settings__created_by",
                schema: "notice",
                table: "notification_settings",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_notification_settings__deleted_by",
                schema: "notice",
                table: "notification_settings",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_notification_settings__updated_by",
                schema: "notice",
                table: "notification_settings",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_notification_settings_notification_type_id",
                schema: "notice",
                table: "notification_settings",
                column: "notification_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_settings_user_id",
                schema: "notice",
                table: "notification_settings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications__created_by",
                schema: "notice",
                table: "notifications",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_notifications__deleted_by",
                schema: "notice",
                table: "notifications",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_notifications__updated_by",
                schema: "notice",
                table: "notifications",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_content",
                schema: "notice",
                table: "notifications",
                column: "content");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_notification_type_id",
                schema: "notice",
                table: "notifications",
                column: "notification_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_send_date",
                schema: "notice",
                table: "notifications",
                column: "send_date");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_user_id",
                schema: "notice",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_info_email",
                schema: "app",
                table: "users_info",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_info_name",
                schema: "app",
                table: "users_info",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_info_source_id",
                schema: "app",
                table: "users_info",
                column: "source_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_settings",
                schema: "notice");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "notice");

            migrationBuilder.DropTable(
                name: "notification_types",
                schema: "notice");

            migrationBuilder.DropTable(
                name: "users_info",
                schema: "app");

            migrationBuilder.DropTable(
                name: "sources",
                schema: "app");
        }
    }
}
