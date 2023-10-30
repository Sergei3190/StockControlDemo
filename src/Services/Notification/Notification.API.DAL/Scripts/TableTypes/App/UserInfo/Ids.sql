if (SELECT type_table_object_id  FROM sys.table_types WHERE name = 'users_info_ids_type') is null
begin
    create type [app].[users_info_ids_type] as table
	(
		id uniqueidentifier
	)
end