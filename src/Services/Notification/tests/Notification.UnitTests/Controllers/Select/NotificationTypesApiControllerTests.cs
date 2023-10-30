using Notification.API.Controllers.Select;
using Notification.API.MediatR.Queries;
using Notification.API.Models.DTO.NotificationType;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace Notification.UnitTests.Controllers.Select;

public class NotificationTypesApiControllerTests
{
	private readonly Mock<IMediator> _mediator;
	private readonly Mock<ILogger<NotificationTypesApiController>> _logger;

	public NotificationTypesApiControllerTests()
	{
		_mediator = new Mock<IMediator>();
		_logger = new Mock<ILogger<NotificationTypesApiController>>();
	}

	[Fact]
	public async Task Select_should_return_no_content_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var paginatedItems = new PaginatedItemsDto<NamedEntityDto>();

		_mediator.Setup(m => m.Send(It.IsAny<GetNotificationTypesQuery>(), default))
			.ReturnsAsync(paginatedItems);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationTypesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Select(It.IsAny<NotificationTypeFilterDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NoContentResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNotificationTypesQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Select_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var items = new List<NamedEntityDto>()
		{
			new NamedEntityDto()
			{
				Id = Guid.NewGuid(),
				Name = "Test"
			}
		};

		var paginatedItems = new PaginatedItemsDto<NamedEntityDto>(0, 10, items.Count, items);

		_mediator.Setup(m => m.Send(It.IsAny<GetNotificationTypesQuery>(), default))
			.ReturnsAsync(paginatedItems);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotificationTypesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Select(It.IsAny<NotificationTypeFilterDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultPaginatedItems = result.Value as PaginatedItemsDto<NamedEntityDto>;
		Assert.Equal(paginatedItems.Page, resultPaginatedItems!.Page);
		Assert.Equal(paginatedItems.PageSize, resultPaginatedItems.PageSize);
		Assert.Equal(paginatedItems.TotalItems, resultPaginatedItems.TotalItems);
		Assert.Equal(paginatedItems.Items.Count(), resultPaginatedItems.Items.Count());
		Assert.Equal(paginatedItems.Items.Select(i => i), resultPaginatedItems.Items.Select(i => i));

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNotificationTypesQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}
}
