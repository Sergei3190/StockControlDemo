if (SELECT OBJECT_ID FROM sys.procedures WHERE name = 'default_notification_settings_proc') is null
begin
    exec('create procedure [notice].[default_notification_settings_proc]
		 (
		     @ids [app].[users_info_ids_type] readonly
		 )
		 as
		 begin
			set nocount on;

			declare @user_ids table 
			(
				id uniqueidentifier
			);

			insert into @user_ids (id)
			select id from @ids

			while(select count(*) from @user_ids) > 0
			begin 
				declare @id uniqueidentifier;

				select top 1 @id = id from @user_ids

				insert into [notice].[notification_settings] ([notification_type_id], [user_id], [enable])
				select t.id as notification_type_id,
						@id as user_id,
						0 as enable
				from [notice].[notification_types] as t

				delete from @user_ids
				where id = @id
			end
		 end');
end