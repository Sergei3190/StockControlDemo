using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Note.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "note");

            migrationBuilder.EnsureSchema(
                name: "app");

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
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 4, 48, 412, DateTimeKind.Unspecified).AddTicks(2399), new TimeSpan(0, 4, 0, 0, 0))),
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
                name: "notes",
                schema: "note",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    content = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    is_fix = table.Column<bool>(type: "bit", nullable: false),
                    sort = table.Column<int>(type: "int", nullable: false),
                    execution_date = table.Column<DateTime>(type: "date", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 7, 20, 17, 4, 48, 412, DateTimeKind.Unspecified).AddTicks(8821), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes", x => x.id);
                    table.ForeignKey(
                        name: "FK_notes_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notes_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notes_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notes_users_info_user_id",
                        column: x => x.user_id,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_notes__created_by",
                schema: "note",
                table: "notes",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_notes__deleted_by",
                schema: "note",
                table: "notes",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_notes__updated_by",
                schema: "note",
                table: "notes",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_notes_content",
                schema: "note",
                table: "notes",
                column: "content");

            migrationBuilder.CreateIndex(
                name: "IX_notes_user_id",
                schema: "note",
                table: "notes",
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
                name: "notes",
                schema: "note");

            migrationBuilder.DropTable(
                name: "users_info",
                schema: "app");

            migrationBuilder.DropTable(
                name: "sources",
                schema: "app");
        }
    }
}
