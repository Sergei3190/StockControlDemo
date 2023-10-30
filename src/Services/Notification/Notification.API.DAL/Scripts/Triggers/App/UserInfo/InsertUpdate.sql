if (SELECT OBJECT_ID FROM sys.triggers WHERE name = 'users_info_insert_update') is null
begin
    exec('create trigger [app].[users_info_insert_update]
          on [app].[users_info]
          after insert, update
          as
          begin
              declare @ids [app].[users_info_ids_type];

	          insert into @ids ([id])
              select id from inserted

              exec [notice].[default_notification_settings_proc] @ids = @ids
          end');
end