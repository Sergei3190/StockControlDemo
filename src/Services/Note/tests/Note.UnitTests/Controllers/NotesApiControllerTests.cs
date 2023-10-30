using System.Net;

using Microsoft.AspNetCore.Mvc;

using Note.API.Controllers;
using Note.API.MediatR.Commands;
using Note.API.MediatR.Queries;
using Note.API.Models.DTO;

using Service.Common.DTO;

namespace Note.UnitTests.Controllers;

public class NotesApiControllerTests
{
	private readonly Mock<IMediator> _mediator;
	private readonly Mock<ILogger<NotesApiController>> _logger;

	public NotesApiControllerTests()
	{
		_mediator = new Mock<IMediator>();
		_logger = new Mock<ILogger<NotesApiController>>();
	}

	[Fact]
	public async Task Get_list_should_return_no_content_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var paginatedItems = new PaginatedItemsDto<NoteDto>();

		_mediator.Setup(m => m.Send(It.IsAny<GetNotesQuery>(), default))
			.ReturnsAsync(paginatedItems);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.GetList(It.IsAny<NoteFilterDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NoContentResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNotesQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Get_list_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var items = new List<NoteDto>()
		{
			new NoteDto()
			{
				Id = Guid.NewGuid(),
				Content = "Заметка 1",
				IsFix = true,
				Sort = 0,
				ExecutionDate = DateOnly.FromDateTime(DateTime.Now)
			},
			new NoteDto()
			{
				Id = Guid.NewGuid(),
				Content = "Заметка 2",
				IsFix = false,
				Sort = 0,
				ExecutionDate = DateOnly.FromDateTime(DateTime.Now.AddSeconds(5))
			},
			new NoteDto()
			{
				Id = Guid.NewGuid(),
				Content = "Заметка 3",
				IsFix = true,
				Sort = 1,
				ExecutionDate = DateOnly.FromDateTime(DateTime.Now.AddMinutes(2))
			},
		};

		var paginatedItems = new PaginatedItemsDto<NoteDto>(0, 10, items.Count, items);

		_mediator.Setup(m => m.Send(It.IsAny<GetNotesQuery>(), default))
			.ReturnsAsync(paginatedItems);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.GetList(It.IsAny<NoteFilterDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultPaginatedItems = result.Value as PaginatedItemsDto<NoteDto>;
		Assert.Equal(paginatedItems.Page, resultPaginatedItems!.Page);
		Assert.Equal(paginatedItems.PageSize, resultPaginatedItems.PageSize);
		Assert.Equal(paginatedItems.TotalItems, resultPaginatedItems.TotalItems);
		Assert.Equal(paginatedItems.Items.Count(), resultPaginatedItems.Items.Count());
		Assert.Equal(paginatedItems.Items.Select(i => i), resultPaginatedItems.Items.Select(i => i));

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNotesQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Get_by_id_should_return_not_found_result()
	{
		// Arrange
		// Подготавливаем данные для теста

		_mediator.Setup(m => m.Send(It.IsAny<GetNoteByIdQuery>(), default))
			.ReturnsAsync(default(NoteDto));

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.GetById(It.IsAny<Guid>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NotFoundResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNoteByIdQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Get_by_id_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var dto = new NoteDto()
		{
			Id = Guid.NewGuid(),
			Content = "Заметка 1",
			IsFix = true,
			Sort = 0,
			ExecutionDate = DateOnly.FromDateTime(DateTime.Now)
		};

		_mediator.Setup(m => m.Send(It.IsAny<GetNoteByIdQuery>(), default))
			.ReturnsAsync(dto);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.GetById(It.IsAny<Guid>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultDto = result.Value as NoteDto;
		Assert.NotNull(resultDto);
		Assert.Equal(dto, resultDto);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<GetNoteByIdQuery>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Greate_should_return_create_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var dto = new NoteDto()
		{
			Id = Guid.NewGuid(),
			Content = "Заметка 1",
			IsFix = true,
			Sort = 0,
			ExecutionDate = DateOnly.FromDateTime(DateTime.Now)
		};

		var actionName = "GetById";
		var keyName = "id";

		_mediator.Setup(m => m.Send(It.IsAny<CreateNoteCommand>(), default))
			.ReturnsAsync(dto.Id);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Create(It.IsAny<NoteDto>());

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
		_mediator.Verify(m => m.Send(It.IsAny<CreateNoteCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Update_should_return_not_found_result()
	{
		// Arrange
		// Подготавливаем данные для теста

		_mediator.Setup(m => m.Send(It.IsAny<UpdateNoteCommand>(), default))
			.ReturnsAsync(false);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Update(It.IsAny<NoteDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NotFoundResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<UpdateNoteCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Update_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var flag = true;

		_mediator.Setup(m => m.Send(It.IsAny<UpdateNoteCommand>(), default))
			.ReturnsAsync(flag);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Update(It.IsAny<NoteDto>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultFlag = result.Value is bool;
		Assert.Equal(flag, resultFlag);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<UpdateNoteCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Update_sort_should_return_not_found_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		_mediator.Setup(m => m.Send(It.IsAny<UpdateSortCommand>(), default))
			.ReturnsAsync(false);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.UpdateSort(It.IsAny<NoteDto[]>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NotFoundResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<UpdateSortCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Update_sort_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var flag = true;

		_mediator.Setup(m => m.Send(It.IsAny<UpdateSortCommand>(), default))
			.ReturnsAsync(flag);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.UpdateSort(It.IsAny<NoteDto[]>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultFlag = result.Value is bool;
		Assert.Equal(flag, resultFlag);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<UpdateSortCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Delete_should_return_not_found_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		_mediator.Setup(m => m.Send(It.IsAny<DeleteNoteCommand>(), default))
			.ReturnsAsync(false);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Delete(It.IsAny<Guid>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<NotFoundResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<DeleteNoteCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task Delete_should_return_ok_result()
	{
		// Arrange
		// Подготавливаем данные для теста
		var flag = true;

		_mediator.Setup(m => m.Send(It.IsAny<DeleteNoteCommand>(), default))
			.ReturnsAsync(flag);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат
		var controller = new NotesApiController(_mediator.Object, _logger.Object);
		var actionResult = controller.Delete(It.IsAny<Guid>());

		// Assert
		// Проверяем результат
		Assert.NotNull(actionResult);

		var result = Assert.IsType<OkObjectResult>(actionResult.Result);
		Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

		var resultFlag = result.Value is bool;
		Assert.Equal(flag, resultFlag);

		// проверяем вызов метода в тестируемом методе
		_mediator.Verify(m => m.Send(It.IsAny<DeleteNoteCommand>(), default));
		// проверяем что только он вызывался
		_mediator.VerifyNoOtherCalls();
	}
}