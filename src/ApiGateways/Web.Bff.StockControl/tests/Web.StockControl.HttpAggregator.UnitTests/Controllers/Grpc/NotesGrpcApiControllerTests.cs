using Web.StockControl.HttpAggregator.Controllers.Grpc;
using Web.StockControl.HttpAggregator.Grpc.ClientServices.Intefaces;
using Web.StockControl.HttpAggregator.Grpc.Models;

namespace Web.StockControl.HttpAggregator.UnitTests.Controllers.Grpc;

public class NotesGrpcApiControllerTests
{
	private readonly Mock<INoteGrpcClientService> _clientService;
	private readonly Mock<ILogger<NotesGrpcApiController>> _logger;

	public NotesGrpcApiControllerTests()
	{
		_clientService = new Mock<INoteGrpcClientService>();
		_logger = new Mock<ILogger<NotesGrpcApiController>>();
	}

	[Fact]
	public async Task Update_sort_should_return_not_found_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		_clientService.Setup(m => m.UpdateSortAsync(It.IsAny<NoteArrayItemModel[]>()))
			.ReturnsAsync(false);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesGrpcApiController(_clientService.Object, _logger.Object);
		var actionResult = controller.UpdateSort(It.IsAny<NoteArrayItemModel[]>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NotFoundResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_clientService.Verify(m => m.UpdateSortAsync(It.IsAny<NoteArrayItemModel[]>()));
		// проверяем что только он вызывался
		_clientService.VerifyNoOtherCalls();		
	}

	[Fact]
	public async Task Update_sort_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var flag = true;

		_clientService.Setup(m => m.UpdateSortAsync(It.IsAny<NoteArrayItemModel[]>()))
			.ReturnsAsync(flag);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesGrpcApiController(_clientService.Object, _logger.Object);
		var actionResult = controller.UpdateSort(It.IsAny<NoteArrayItemModel[]>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultFlag = result.Value is bool;
		Assert.Equal(flag, resultFlag);

		// проверяем вызов метода в тестируемом методе
		_clientService.Verify(m => m.UpdateSortAsync(It.IsAny<NoteArrayItemModel[]>()));
		// проверяем что только он вызывался
		_clientService.VerifyNoOtherCalls();
	}
}