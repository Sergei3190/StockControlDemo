using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockControl.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "stock");

            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "classifiers",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mnemo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classifiers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_flow_types",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mnemo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_flow_types", x => x.id);
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
                name: "nomenclatures",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    classifier_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					_created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 9, DateTimeKind.Unspecified).AddTicks(1109), new TimeSpan(0, 4, 0, 0, 0))),
					_created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				},
                constraints: table =>
                {
                    table.PrimaryKey("PK_nomenclatures", x => x.id);
                    table.ForeignKey(
                        name: "FK_nomenclatures_classifiers_classifier_id",
                        column: x => x.classifier_id,
                        principalSchema: "stock",
                        principalTable: "classifiers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_nomenclatures_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_nomenclatures_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_nomenclatures_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                schema: "stock",
                columns: table => new
                {
					id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
					name = table.Column<string>(type: "nvarchar(450)", nullable: false),
					classifier_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					_created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 9, DateTimeKind.Unspecified).AddTicks(1109), new TimeSpan(0, 4, 0, 0, 0))),
					_created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				},
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.id);
                    table.ForeignKey(
                        name: "FK_organizations_classifiers_classifier_id",
                        column: x => x.classifier_id,
                        principalSchema: "stock",
                        principalTable: "classifiers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_organizations_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_organizations_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_organizations_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "parties",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    extension_number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    create_date = table.Column<DateTime>(type: "date", nullable: false),
                    create_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 21, DateTimeKind.Unspecified).AddTicks(5163), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parties", x => x.id);
                    table.ForeignKey(
                        name: "FK_parties_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_parties_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_parties_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                schema: "stock",
                columns: table => new
                {
					id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
					name = table.Column<string>(type: "nvarchar(450)", nullable: false),
					classifier_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					_created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 9, DateTimeKind.Unspecified).AddTicks(1109), new TimeSpan(0, 4, 0, 0, 0))),
					_created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					_deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					_deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				},
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouses", x => x.id);
                    table.ForeignKey(
                        name: "FK_warehouses_classifiers_classifier_id",
                        column: x => x.classifier_id,
                        principalSchema: "stock",
                        principalTable: "classifiers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_warehouses_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_warehouses_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_warehouses_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "movings",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
					product_flow_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					number = table.Column<string>(type: "nvarchar(450)", nullable: false),
					create_date = table.Column<DateTime>(type: "date", nullable: false),
					create_time = table.Column<TimeSpan>(type: "time", nullable: false),
					sending_warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					party_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					nomenclature_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					organization_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					quantity = table.Column<int>(type: "int", nullable: false),
					total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					_created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 14, DateTimeKind.Unspecified).AddTicks(5099), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movings", x => x.id);
                    table.ForeignKey(
                        name: "FK_movings_nomenclatures_nomenclature_id",
                        column: x => x.nomenclature_id,
                        principalSchema: "stock",
                        principalTable: "nomenclatures",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_movings_organizations_organization_id",
                        column: x => x.organization_id,
                        principalSchema: "stock",
                        principalTable: "organizations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_movings_parties_party_id",
                        column: x => x.party_id,
                        principalSchema: "stock",
                        principalTable: "parties",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_movings_product_flow_types_product_flow_type_id",
                        column: x => x.product_flow_type_id,
                        principalSchema: "stock",
                        principalTable: "product_flow_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_movings_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_movings_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_movings_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_movings_warehouses_sending_warehouse_id",
                        column: x => x.sending_warehouse_id,
                        principalSchema: "stock",
                        principalTable: "warehouses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_movings_warehouses_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "stock",
                        principalTable: "warehouses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "receipts",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
					product_flow_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					number = table.Column<string>(type: "nvarchar(450)", nullable: false),
					create_date = table.Column<DateTime>(type: "date", nullable: false),
					create_time = table.Column<TimeSpan>(type: "time", nullable: false),
					party_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					nomenclature_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					organization_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					quantity = table.Column<int>(type: "int", nullable: false),
					total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					_created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 11, DateTimeKind.Unspecified).AddTicks(3371), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipts", x => x.id);
                    table.ForeignKey(
                        name: "FK_receipts_nomenclatures_nomenclature_id",
                        column: x => x.nomenclature_id,
                        principalSchema: "stock",
                        principalTable: "nomenclatures",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receipts_organizations_organization_id",
                        column: x => x.organization_id,
                        principalSchema: "stock",
                        principalTable: "organizations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receipts_parties_party_id",
                        column: x => x.party_id,
                        principalSchema: "stock",
                        principalTable: "parties",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receipts_product_flow_types_product_flow_type_id",
                        column: x => x.product_flow_type_id,
                        principalSchema: "stock",
                        principalTable: "product_flow_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receipts_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receipts_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receipts_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_receipts_warehouses_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "stock",
                        principalTable: "warehouses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "write_offs",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
					product_flow_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					number = table.Column<string>(type: "nvarchar(450)", nullable: false),
					create_date = table.Column<DateTime>(type: "date", nullable: false),
					create_time = table.Column<TimeSpan>(type: "time", nullable: false),
					sending_warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					party_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					nomenclature_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					organization_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					quantity = table.Column<int>(type: "int", nullable: false),
					total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 17, DateTimeKind.Unspecified).AddTicks(9243), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_write_offs", x => x.id);
                    table.ForeignKey(
                        name: "FK_write_offs_nomenclatures_nomenclature_id",
                        column: x => x.nomenclature_id,
                        principalSchema: "stock",
                        principalTable: "nomenclatures",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_write_offs_organizations_organization_id",
                        column: x => x.organization_id,
                        principalSchema: "stock",
                        principalTable: "organizations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_write_offs_parties_party_id",
                        column: x => x.party_id,
                        principalSchema: "stock",
                        principalTable: "parties",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_write_offs_product_flow_types_product_flow_type_id",
                        column: x => x.product_flow_type_id,
                        principalSchema: "stock",
                        principalTable: "product_flow_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_write_offs_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_write_offs_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_write_offs_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_write_offs_warehouses_sending_warehouse_id",
                        column: x => x.sending_warehouse_id,
                        principalSchema: "stock",
                        principalTable: "warehouses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_write_offs_warehouses_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "stock",
                        principalTable: "warehouses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "stock_availabilities",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
					party_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					nomenclature_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					organization_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					quantity = table.Column<int>(type: "int", nullable: false),
					total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					receipt_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    moving_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    write_off_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 10, 13, 11, 54, 45, 22, DateTimeKind.Unspecified).AddTicks(9886), new TimeSpan(0, 4, 0, 0, 0))),
                    _created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _deleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    _deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_availabilities", x => x.id);
                    table.ForeignKey(
                        name: "FK_stock_availabilities_movings_moving_id",
                        column: x => x.moving_id,
                        principalSchema: "stock",
                        principalTable: "movings",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_nomenclatures_nomenclature_id",
                        column: x => x.nomenclature_id,
                        principalSchema: "stock",
                        principalTable: "nomenclatures",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_organizations_organization_id",
                        column: x => x.organization_id,
                        principalSchema: "stock",
                        principalTable: "organizations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_parties_party_id",
                        column: x => x.party_id,
                        principalSchema: "stock",
                        principalTable: "parties",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_receipts_receipt_id",
                        column: x => x.receipt_id,
                        principalSchema: "stock",
                        principalTable: "receipts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_users_info__created_by",
                        column: x => x._created_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_users_info__deleted_by",
                        column: x => x._deleted_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_users_info__updated_by",
                        column: x => x._updated_by,
                        principalSchema: "app",
                        principalTable: "users_info",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_warehouses_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "stock",
                        principalTable: "warehouses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_availabilities_write_offs_write_off_id",
                        column: x => x.write_off_id,
                        principalSchema: "stock",
                        principalTable: "write_offs",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_movings__created_by",
                schema: "stock",
                table: "movings",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_movings__deleted_by",
                schema: "stock",
                table: "movings",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_movings__updated_by",
                schema: "stock",
                table: "movings",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_movings_nomenclature_id",
                schema: "stock",
                table: "movings",
                column: "nomenclature_id");

            migrationBuilder.CreateIndex(
                name: "IX_movings_organization_id",
                schema: "stock",
                table: "movings",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_movings_party_id",
                schema: "stock",
                table: "movings",
                column: "party_id");

            migrationBuilder.CreateIndex(
                name: "IX_movings_product_flow_type_id_number_create_date_create_time",
                schema: "stock",
                table: "movings",
                columns: new[] { "product_flow_type_id", "number", "create_date", "create_time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_movings_sending_warehouse_id",
                schema: "stock",
                table: "movings",
                column: "sending_warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_movings_warehouse_id",
                schema: "stock",
                table: "movings",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_nomenclatures__created_by",
                schema: "stock",
                table: "nomenclatures",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_nomenclatures__deleted_by",
                schema: "stock",
                table: "nomenclatures",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_nomenclatures__updated_by",
                schema: "stock",
                table: "nomenclatures",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_nomenclatures_classifier_id",
                schema: "stock",
                table: "nomenclatures",
                column: "classifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_nomenclatures_name",
                schema: "stock",
                table: "nomenclatures",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_organizations__created_by",
                schema: "stock",
                table: "organizations",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_organizations__deleted_by",
                schema: "stock",
                table: "organizations",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_organizations__updated_by",
                schema: "stock",
                table: "organizations",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_organizations_classifier_id",
                schema: "stock",
                table: "organizations",
                column: "classifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_organizations_name",
                schema: "stock",
                table: "organizations",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_parties__created_by",
                schema: "stock",
                table: "parties",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_parties__deleted_by",
                schema: "stock",
                table: "parties",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_parties__updated_by",
                schema: "stock",
                table: "parties",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_parties_extension_number",
                schema: "stock",
                table: "parties",
                column: "extension_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parties_number",
                schema: "stock",
                table: "parties",
                column: "number");

            migrationBuilder.CreateIndex(
                name: "IX_receipts__created_by",
                schema: "stock",
                table: "receipts",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_receipts__deleted_by",
                schema: "stock",
                table: "receipts",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_receipts__updated_by",
                schema: "stock",
                table: "receipts",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_receipts_nomenclature_id",
                schema: "stock",
                table: "receipts",
                column: "nomenclature_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipts_organization_id",
                schema: "stock",
                table: "receipts",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipts_party_id",
                schema: "stock",
                table: "receipts",
                column: "party_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_receipts_product_flow_type_id_number_create_date_create_time",
                schema: "stock",
                table: "receipts",
                columns: new[] { "product_flow_type_id", "number", "create_date", "create_time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_receipts_warehouse_id",
                schema: "stock",
                table: "receipts",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities__created_by",
                schema: "stock",
                table: "stock_availabilities",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities__deleted_by",
                schema: "stock",
                table: "stock_availabilities",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities__updated_by",
                schema: "stock",
                table: "stock_availabilities",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities_moving_id",
                schema: "stock",
                table: "stock_availabilities",
                column: "moving_id",
                unique: true,
                filter: "[moving_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities_nomenclature_id",
                schema: "stock",
                table: "stock_availabilities",
                column: "nomenclature_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities_organization_id",
                schema: "stock",
                table: "stock_availabilities",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities_party_id",
                schema: "stock",
                table: "stock_availabilities",
                column: "party_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities_receipt_id",
                schema: "stock",
                table: "stock_availabilities",
                column: "receipt_id",
                unique: true,
                filter: "[receipt_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities_warehouse_id",
                schema: "stock",
                table: "stock_availabilities",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_availabilities_write_off_id",
                schema: "stock",
                table: "stock_availabilities",
                column: "write_off_id",
                unique: true,
                filter: "[write_off_id] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_warehouses__created_by",
                schema: "stock",
                table: "warehouses",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_warehouses__deleted_by",
                schema: "stock",
                table: "warehouses",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_warehouses__updated_by",
                schema: "stock",
                table: "warehouses",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_warehouses_classifier_id",
                schema: "stock",
                table: "warehouses",
                column: "classifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_warehouses_name",
                schema: "stock",
                table: "warehouses",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_write_offs__created_by",
                schema: "stock",
                table: "write_offs",
                column: "_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_write_offs__deleted_by",
                schema: "stock",
                table: "write_offs",
                column: "_deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_write_offs__updated_by",
                schema: "stock",
                table: "write_offs",
                column: "_updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_write_offs_nomenclature_id",
                schema: "stock",
                table: "write_offs",
                column: "nomenclature_id");

            migrationBuilder.CreateIndex(
                name: "IX_write_offs_organization_id",
                schema: "stock",
                table: "write_offs",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_write_offs_party_id",
                schema: "stock",
                table: "write_offs",
                column: "party_id");

            migrationBuilder.CreateIndex(
                name: "IX_write_offs_product_flow_type_id_number_create_date_create_time",
                schema: "stock",
                table: "write_offs",
                columns: new[] { "product_flow_type_id", "number", "create_date", "create_time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_write_offs_sending_warehouse_id",
                schema: "stock",
                table: "write_offs",
                column: "sending_warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_write_offs_warehouse_id",
                schema: "stock",
                table: "write_offs",
                column: "warehouse_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stock_availabilities",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "movings",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "receipts",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "write_offs",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "nomenclatures",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "organizations",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "parties",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "product_flow_types",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "warehouses",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "classifiers",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "users_info",
                schema: "app");

            migrationBuilder.DropTable(
                name: "sources",
                schema: "app");
        }
    }
}
