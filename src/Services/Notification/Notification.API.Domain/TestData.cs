using Service.Common.Entities.App;

namespace Notification.API.Domain;

public static class TestData
{
	private const string TestAdminId = "0E97468A-9710-48FB-B6C4-FCEB9C17D6D5";

	public static IEnumerable<UserInfo> Users { get; } = new List<UserInfo>()
	{
		new UserInfo()
		{
			Id = Guid.Parse(TestAdminId),
			Name = "Test",
			Email = "Test Email",
			SourceId = Source.Sources.First().Id
		},
	};
}