insert into [app].[users_info] ([id], [name], [email], [source_id])
values ('8CDF76BA-F226-EE11-9A9A-D0C5D371B064', N'name 1', N'email 1', '73F19E6B-73AD-462B-AC5D-0F3464815314'),
       ('8DDF76BA-F226-EE11-9A9A-D0C5D371B064', N'name 2', N'email 2', '73F19E6B-73AD-462B-AC5D-0F3464815314')

select ns.[id] as id, 
	   ui.[id] as new_user_id,
	   COUNT(ns.id) as row_count
from [notice].[notification_settings] as ns
join [app].[users_info] as ui on ns.[user_id] = ui.[id] and ui.id in ('8CDF76BA-F226-EE11-9A9A-D0C5D371B064', '8DDF76BA-F226-EE11-9A9A-D0C5D371B064')
group by ns.id, ui.id

if (@@ROWCOUNT = 6)
begin
    delete from [notice].[notification_settings] where [user_id] in ('8CDF76BA-F226-EE11-9A9A-D0C5D371B064', '8DDF76BA-F226-EE11-9A9A-D0C5D371B064')
    delete from [app].[users_info] where [id] in ('8CDF76BA-F226-EE11-9A9A-D0C5D371B064', '8DDF76BA-F226-EE11-9A9A-D0C5D371B064')
end
