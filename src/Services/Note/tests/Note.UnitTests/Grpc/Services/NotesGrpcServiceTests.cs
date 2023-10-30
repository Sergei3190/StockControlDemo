using Grpc.Core;

using GrpcNote;

using Note.API.MediatR.Commands;

namespace Note.UnitTests.Grpc.Services;

public class NotesGrpcServiceTests
{
	private readonly Mock<IMediator> _mediator;
	private readonly Mock<ILogger<NotesGrpcService>> _logger;
	private readonly Mock<ServerCallContext> _context;

	public NotesGrpcServiceTests()
	{
		_mediator = new Mock<IMediator>();
		_context = new Mock<ServerCallContext>();
		_logger = new Mock<ILogger<NotesGrpcService>>();
	}

	[Fact]
	public async Task Update_sort_async_should_end_failed()
	{
		// Arrange
		// Подготавливаем данные для теста
		var paramName = "request";

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NotesGrpcService(_mediator.Object, _logger.Object);
		var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateSort(null!, _context.Object).ConfigureAwait(false));

		// Assert
		// Проверяем результат
		Assert.NotNull(exception);
		Assert.Equal(paramName, exception.Result.ParamName);
	}

	[Fact]
	public async Task Update_sort_async_wrong_data_should_return_false()
	{
		// Arrange
		// Подготавливаем данные для теста
		var request = new NoteArrayRequest();
		var responseFlag = false;

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NotesGrpcService(_mediator.Object, _logger.Object);
		var result = await service.UpdateSort(request, _context.Object);

		// Assert
		// Проверяем результат
		Assert.NotNull(result);
		Assert.IsType<NoteArrayResponse>(result);
		Assert.Equal(responseFlag, result.Flag);
	}

	[Fact]
	public async Task Update_sort_async_data_not_found_should_return_false()
	{
		// Arrange
		// Подготавливаем данные для теста
		var request = new NoteArrayRequest();
		request.Items.Add(new NoteArrayItemRequest()
		{
			Id = Guid.NewGuid().ToString(),
			Content = "Заметка",
			IsFix = true,
			Sort = 1,
			ExecutionDate = Convert.ToString(DateOnly.FromDateTime(DateTime.Now))
		});
		var responseFlag = false;

		_mediator.Setup(m => m.Send(It.IsAny<UpdateSortCommand>(), default))
			.ReturnsAsync(false);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NotesGrpcService(_mediator.Object, _logger.Object);
		var result = await service.UpdateSort(request, _context.Object);

		// Assert
		// Проверяем результат
		Assert.NotNull(result);
		Assert.IsType<NoteArrayResponse>(result);
		Assert.Equal(responseFlag, result.Flag);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<UpdateSortCommand>(), default));
	}

	[Fact]
	public async Task Update_sort_async_should_return_true()
	{
		// Arrange
		// Подготавливаем данные для теста
		var request = new NoteArrayRequest();
		request.Items.Add(new NoteArrayItemRequest()
		{
			Id = Guid.NewGuid().ToString(),
			Content = "Заметка",
			IsFix = true,
			Sort = 1,
			ExecutionDate = Convert.ToString(DateOnly.FromDateTime(DateTime.Now))
		});
		var responseFlag = true;

		_mediator.Setup(m => m.Send(It.IsAny<UpdateSortCommand>(), default))
			.ReturnsAsync(true);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NotesGrpcService(_mediator.Object, _logger.Object);
		var result = await service.UpdateSort(request, _context.Object);

		// Assert
		// Проверяем результат
		Assert.NotNull(result);
		Assert.IsType<NoteArrayResponse>(result);
		Assert.Equal(responseFlag, result.Flag);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<UpdateSortCommand>(), default));
	}
}