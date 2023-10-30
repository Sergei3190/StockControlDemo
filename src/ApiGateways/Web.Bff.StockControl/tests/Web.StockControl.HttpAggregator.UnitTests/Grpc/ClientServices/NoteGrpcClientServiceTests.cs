using Grpc.Core;

using GrpcNote;

using Web.StockControl.HttpAggregator.Grpc.ClientServices;
using Web.StockControl.HttpAggregator.Grpc.Models;

namespace Web.StockControl.HttpAggregator.UnitTests.Grpc.ClientServices;

public class NoteGrpcClientServiceTests
{
	private Mock<Note.NoteClient> _client;
	private Mock<ILogger<NoteGrpcClientService>> _logger;

	public NoteGrpcClientServiceTests()
	{
		_client = new Mock<Note.NoteClient>();
		_logger = new Mock<ILogger<NoteGrpcClientService>>();
	}

	[Fact]
	public async Task Update_sort_async_should_return_true()
	{
		// Arrange
		// Подготавливаем данные для теста
		var dtoArray = new NoteArrayItemModel[]
		{
			new NoteArrayItemModel()
			{
				Id = Guid.NewGuid().ToString(),
				Content = "Заметка",
				IsFix = true,
				Sort = 1,
				ExecutionDate = Convert.ToString(DateOnly.FromDateTime(DateTime.Now))
			}
		};

		var result = true;

		_client.Setup(c => c.UpdateSortAsync(It.IsAny<NoteArrayRequest>(), null, null, CancellationToken.None))
			.Returns(new AsyncUnaryCall<NoteArrayResponse>(
				Task.FromResult(new NoteArrayResponse() { Flag = true }),
				Task.FromResult(new Metadata()),
				() => Status.DefaultSuccess,
				() => new Metadata(),
				() => { }));

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NoteGrpcClientService(_client.Object, _logger.Object);
		var actionResult = await service.UpdateSortAsync(dtoArray).ConfigureAwait(false);

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);
		Assert.Equal(result, actionResult);

		// проверяем вызов метода в тестируемом методе
		_client.Verify(m => m.UpdateSortAsync(It.IsAny<NoteArrayRequest>(), null, null, CancellationToken.None));
	}

	[Fact]
	public async Task Update_sort_async_should_return_false()
	{
		// Arrange
		// Подготавливаем данные для теста
		var dtoArray = new NoteArrayItemModel[] { };

		var result = false;

		_client.Setup(c => c.UpdateSortAsync(It.IsAny<NoteArrayRequest>(), null, null, CancellationToken.None))
			.Returns(new AsyncUnaryCall<NoteArrayResponse>(
				Task.FromResult(new NoteArrayResponse() { Flag = false }),
				Task.FromResult(new Metadata()),
				() => Status.DefaultSuccess,
				() => new Metadata(),
				() => { }));

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NoteGrpcClientService(_client.Object, _logger.Object);
		var actionResult = await service.UpdateSortAsync(dtoArray).ConfigureAwait(false);

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);
		Assert.Equal(result, actionResult);

		// проверяем вызов метода в тестируемом методе
		_client.Verify(m => m.UpdateSortAsync(It.IsAny<NoteArrayRequest>(), null, null, CancellationToken.None));
	}
}
