using Notification.API.Controllers;
using Notification.API.MediatR.Commands;
using Notification.API.MediatR.Queries;
using Notification.API.Models.DTO.NotificationSetting;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace Notification.UnitTests.Controllers;

public class NotificationSettingsApiControllerTests : ControllerBase
{
	private readonly Mock<IMediator> _mediator;
	private readonly Mock<ILogger<NotificationSettingsApiController>> _logger;

	public NotificationSettingsApiControllerTests()
	{
		_mediator = new Mock<IMediator>();
		_logger = new Mock<ILogger<NotificationSettingsApiController>>();
	}

	[Fact]
	public async Task Get_list_should_return_no_content_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var paginatedItems = new PaginatedItemsDto<NotificationSettingDto>();

		_mediator.Setup(m => m.Send(It.IsAny<GetNotificationSettingsQuery>(), default))
			.ReturnsAsync(paginatedItems);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.GetList(It.IsAny<NotificationSettingFilterDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NoContentResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNotificationSettingsQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Get_list_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var items = new List<NotificationSettingDto>()
		{
			new NotificationSettingDto()
			{
				Id = Guid.NewGuid(),
				NotificationType = new NamedEntityDto()
				{
					Id = Guid.NewGuid(),
					Name = "Test",
				},
				Enable = true
			},
		};

		var paginatedItems = new PaginatedItemsDto<NotificationSettingDto>(0, 10, items.Count, items);

		_mediator.Setup(m => m.Send(It.IsAny<GetNotificationSettingsQuery>(), default))
			.ReturnsAsync(paginatedItems);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.GetList(It.IsAny<NotificationSettingFilterDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultPaginatedItems = result.Value as PaginatedItemsDto<NotificationSettingDto>;
		Assert.Equal(paginatedItems.Page, resultPaginatedItems!.Page);
		Assert.Equal(paginatedItems.PageSize, resultPaginatedItems.PageSize);
		Assert.Equal(paginatedItems.TotalItems, resultPaginatedItems.TotalItems);
		Assert.Equal(paginatedItems.Items.Count(), resultPaginatedItems.Items.Count());
		Assert.Equal(paginatedItems.Items.Select(i => i), resultPaginatedItems.Items.Select(i => i));

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNotificationSettingsQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Get_by_id_should_return_not_found_result()
	{
		// Arrange
		// Подготавливаем данные для теста

		_mediator.Setup(m => m.Send(It.IsAny<GetNotificationSettingByIdQuery>(), default))
			.ReturnsAsync(default(NotificationSettingDto));

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.GetById(It.IsAny<Guid>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NotFoundResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNotificationSettingByIdQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Get_by_id_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var dto = new NotificationSettingDto()
		{
			Id = Guid.NewGuid(),
			NotificationType = new NamedEntityDto()
			{
				Id = Guid.NewGuid(),
				Name = "Test"
			},
			Enable = true
		};

		_mediator.Setup(m => m.Send(It.IsAny<GetNotificationSettingByIdQuery>(), default))
			.ReturnsAsync(dto);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.GetById(It.IsAny<Guid>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultDto = result.Value as NotificationSettingDto;
		Assert.NotNull(resultDto);
		Assert.Equal(dto, resultDto);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNotificationSettingByIdQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Greate_should_return_create_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var dto = new NotificationSettingDto()
		{
			Id = Guid.NewGuid(),
			NotificationType = new NamedEntityDto()
			{
				Id = Guid.NewGuid(),
				Name = "Test",
			},
			Enable = true
		};

		var actionName = "GetById";
		var keyName = "id";

		_mediator.Setup(m => m.Send(It.IsAny<CreateNotificationSettingCommand>(), default))
			.ReturnsAsync(dto.Id);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Create(It.IsAny<NotificationSettingDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
		Assert.Equal(actionName, result.ActionName);
		Assert.Equal(1, result.RouteValues.Count);
		Assert.Equal(keyName, result.RouteValues.Keys.First());
		Assert.Equal(dto.Id, result.RouteValues[keyName]);
		Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<CreateNotificationSettingCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Update_should_return_not_found_result()
	{
		// Arrange
		// Подготавливаем данные для теста

		_mediator.Setup(m => m.Send(It.IsAny<UpdateNotificationSettingCommand>(), default))
			.ReturnsAsync(false);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Update(It.IsAny<NotificationSettingDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NotFoundResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<UpdateNotificationSettingCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Update_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var flag = true;

		_mediator.Setup(m => m.Send(It.IsAny<UpdateNotificationSettingCommand>(), default))
			.ReturnsAsync(flag);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Update(It.IsAny<NotificationSettingDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultFlag = result.Value is bool;
		Assert.Equal(flag, resultFlag);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<UpdateNotificationSettingCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Delete_should_return_not_found_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		_mediator.Setup(m => m.Send(It.IsAny<DeleteNotificationSettingCommand>(), default))
			.ReturnsAsync(false);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Delete(It.IsAny<Guid>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NotFoundResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<DeleteNotificationSettingCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Delete_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var flag = true;

		_mediator.Setup(m => m.Send(It.IsAny<DeleteNotificationSettingCommand>(), default))
			.ReturnsAsync(flag);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationSettingsApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Delete(It.IsAny<Guid>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultFlag = result.Value is bool;
		Assert.Equal(flag, resultFlag);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<DeleteNotificationSettingCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}
}
