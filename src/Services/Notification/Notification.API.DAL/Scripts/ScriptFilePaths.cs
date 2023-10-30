namespace Notification.API.DAL.Scripts;

/// <summary>
/// Пути к файлам скриптов бд
/// </summary>
public class ScriptFilePaths
{
	public static IEnumerable<string> TableTypes = new List<string>()
	{
		Path.Combine("Scripts","TableTypes","App","UserInfo","Ids.sql"), // кроссплатформенный способ
    };

	public static IEnumerable<string> Procedures = new List<string>()
	{
		Path.Combine("Scripts","Procedure","Notice","NotificationSetting","Default.sql"),
	};

	public static IEnumerable<string> Triggers = new List<string>()
	{
		Path.Combine("Scripts","Triggers","App","UserInfo","InsertUpdate.sql"),
	};

	public static IEnumerable<string> Tests = new List<string>()
	{
		 Path.Combine("Scripts","Triggers","App","UserInfo","Test","InsertUpdateTest.sql"),
	};
}
