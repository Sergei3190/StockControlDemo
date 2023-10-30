using Service.Common.Entities.App;

namespace Note.API.Domain.Note;

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

	public static IEnumerable<UserNote> Notes { get; } = new List<UserNote>()
	{
		new UserNote()
		{
			Id = Guid.NewGuid(),
			Content = "Заметка 1",
			IsFix = true,
			Sort = 0,
			ExecutionDate = DateOnly.FromDateTime(DateTime.Now),
			UserId = Users.First().Id,
			CreatedBy = Users.First().Id,
		},
		new UserNote()
		{
			Id = Guid.NewGuid(),
			Content = "Заметка 2",
			IsFix = false,
			Sort = 0,
			ExecutionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
			UserId = Users.First().Id,
		    CreatedBy = Users.First().Id,
		},
		new UserNote()
		{
			Id = Guid.NewGuid(),
			Content = "Заметка 3",
			IsFix = true,
			Sort = 1,
			ExecutionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
			UserId = Users.First().Id,
			CreatedBy = Users.First().Id,
		},
	};	
}