using Note.API.DAL.Context;
using Note.API.Services;

using Service.Common.Interfaces;

namespace Note.UnitTests.Services;

// тестиреум здесь только то, что явно генерит исключения, обработку возможных ответов данных сервисов мы тестируем в NotesApiControllerTests
public class NotesServiceTests
{
	private readonly Mock<NoteDB> _db;
	private readonly Mock<ISaveService<NoteDB>> _saveService;
	private readonly Mock<IIdentityService> _identityService;
	private readonly Mock<ILogger<NotesService>> _logger;

	public NotesServiceTests()
	{
		_db = new Mock<NoteDB>();
		_saveService = new Mock<ISaveService<NoteDB>>();
		_identityService = new Mock<IIdentityService>();
		_logger = new Mock<ILogger<NotesService>>();
	}

	[Fact]
	public void Create_notes_services_instance_should_end_failed()
	{
		// Arrange
		// Подготавливаем данные для теста
		var errorMesage = "Пользователь не найден";

		_identityService.Setup(m => m.GetUserIdIdentity())
			.Returns(default(Guid?));

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var serviceException = Assert.Throws<InvalidOperationException>(() => new NotesService(_db.Object, _saveService.Object, _identityService.Object, _logger.Object));

		// Assert
		// Проверяем результат
		Assert.NotNull(serviceException);
		Assert.Equal(errorMesage, serviceException.Message);

		// проверяем вызов метода в тестируемом методе
		_identityService.Verify(m => m.GetUserIdIdentity());
		// проверяем что только он вызывался
		_identityService.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Get_list_async_should_end_failed()
	{
		// Arrange
		// Подготавливаем данные для теста
		var paramName = "filter";

		_identityService.Setup(m => m.GetUserIdIdentity())
			.Returns(Guid.NewGuid());

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NotesService(_db.Object, _saveService.Object, _identityService.Object, _logger.Object);
		var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetListAsync(null!).ConfigureAwait(false));

		// Assert
		// Проверяем результат
		Assert.NotNull(exception);
		Assert.Equal(paramName, exception.Result.ParamName);

		// проверяем вызов метода в тестируемом методе
		_identityService.Verify(m => m.GetUserIdIdentity());
	}

	[Fact]
	public async Task Create_async_should_end_failed()
	{
		// Arrange
		// Подготавливаем данные для теста
		var paramName = "dto";

		_identityService.Setup(m => m.GetUserIdIdentity())
			.Returns(Guid.NewGuid());

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NotesService(_db.Object, _saveService.Object, _identityService.Object, _logger.Object);
		var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(null!).ConfigureAwait(false));

		// Assert
		// Проверяем результат
		Assert.NotNull(exception);
		Assert.Equal(paramName, exception.Result.ParamName);

		// проверяем вызов метода в тестируемом методе
		_identityService.Verify(m => m.GetUserIdIdentity());
	}

	[Fact]
	public async Task Update_async_should_end_failed()
	{
		// Arrange
		// Подготавливаем данные для теста
		var paramName = "dto";

		_identityService.Setup(m => m.GetUserIdIdentity())
			.Returns(Guid.NewGuid());

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var service = new NotesService(_db.Object, _saveService.Object, _identityService.Object, _logger.Object);
		var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(null!).ConfigureAwait(false));

		// Assert
		// Проверяем результат
		Assert.NotNull(exception);
		Assert.Equal(paramName, exception.Result.ParamName);

		// проверяем вызов метода в тестируемом методе
		_identityService.Verify(m => m.GetUserIdIdentity());
	}
}