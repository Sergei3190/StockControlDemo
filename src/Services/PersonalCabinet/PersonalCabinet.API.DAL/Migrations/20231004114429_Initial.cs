using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalCabinet.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "person");

            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "cards",
                schema: "person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "loaded_data_types",
                schema: "person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mnemo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loaded_data_types", x => x.id);
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
                    is_lockout = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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
                name: "documents",
                schema: "person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    card_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    external_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loaded_data_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					_created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 4, 15, 44, 28, 973, DateTimeKind.Unspecified).AddTicks(8379), new TimeSpan(0, 4, 0, 0, 0))),
					_created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
				},
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_documents_cards_card_id",
                        column: x => x.card_id,
                        principalSchema: "person",
                        principalTable: "cards",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_documents_loaded_data_types_loaded_data_type_id",
                        column: x => x.loaded_data_type_id,
                        principalSchema: "person",
                        principalTable: "loaded_data_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_documents_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_documents_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_documents_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "persons",
                schema: "person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    middle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age = table.Column<int>(type: "int", nullable: true),
                    birthday = table.Column<DateTime>(type: "date", nullable: true),
                    card_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 4, 15, 44, 28, 970, DateTimeKind.Unspecified).AddTicks(2289), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.id);
                    table.ForeignKey(
                        name: "FK_persons_cards_card_id",
                        column: x => x.card_id,
                        principalSchema: "person",
                        principalTable: "cards",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_persons_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_persons_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_persons_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_persons_users_info_user_id",
                        column: x => x.user_id,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "photos",
                schema: "person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    card_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    external_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loaded_data_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					_created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 4, 15, 44, 28, 972, DateTimeKind.Unspecified).AddTicks(5080), new TimeSpan(0, 4, 0, 0, 0))),
					_created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
				},
                constraints: table =>
                {
                    table.PrimaryKey("PK_photos", x => x.id);
                    table.ForeignKey(
                        name: "FK_photos_cards_card_id",
                        column: x => x.card_id,
                        principalSchema: "person",
                        principalTable: "cards",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_photos_loaded_data_types_loaded_data_type_id",
                        column: x => x.loaded_data_type_id,
                        principalSchema: "person",
                        principalTable: "loaded_data_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_photos_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_photos_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_photos_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_documents__created_by",
                schema: "person",
                table: "documents",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_documents__deleted_by",
                schema: "person",
                table: "documents",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_documents__updated_by",
                schema: "person",
                table: "documents",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_documents_card_id",
                schema: "person",
                table: "documents",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_documents_loaded_data_type_id",
                schema: "person",
                table: "documents",
                column: "loaded_data_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons__created_by",
                schema: "person",
                table: "persons",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_persons__deleted_by",
                schema: "person",
                table: "persons",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_persons__updated_by",
                schema: "person",
                table: "persons",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_persons_card_id",
                schema: "person",
                table: "persons",
                column: "card_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_persons_user_id",
                schema: "person",
                table: "persons",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_photos__created_by",
                schema: "person",
                table: "photos",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_photos__deleted_by",
                schema: "person",
                table: "photos",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_photos__updated_by",
                schema: "person",
                table: "photos",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_photos_card_id",
                schema: "person",
                table: "photos",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_photos_loaded_data_type_id",
                schema: "person",
                table: "photos",
                column: "loaded_data_type_id");

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
                name: "documents",
                schema: "person");

            migrationBuilder.DropTable(
                name: "persons",
                schema: "person");

            migrationBuilder.DropTable(
                name: "photos",
                schema: "person");

            migrationBuilder.DropTable(
                name: "cards",
                schema: "person");

            migrationBuilder.DropTable(
                name: "loaded_data_types",
                schema: "person");

            migrationBuilder.DropTable(
                name: "users_info",
                schema: "app");

            migrationBuilder.DropTable(
                name: "sources",
                schema: "app");
        }
    }
}
