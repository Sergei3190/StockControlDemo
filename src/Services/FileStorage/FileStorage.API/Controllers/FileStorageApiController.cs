using System.Net;
using System.Text;

using FileStorage.API.Infrastructure.Mappers;
using FileStorage.API.Infrastructure.Settings;
using FileStorage.API.MediatR.Commands;
using FileStorage.API.MediatR.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/file-storage")]
public class FileStorageApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger _logger;

	public FileStorageApiController(IMediator mediator, ILogger<FileStorageApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpPost]
	[Route("upload")]
	[DisableRequestSizeLimit] // отключаем лимит размера запроса 
	[RequestFormLimits(MultipartBodyLengthLimit = KestrelLimitSettings.MaxRequestBodySize)] // устанавливаем свой лимит
	public async Task<IActionResult> Upload([FromForm] IFormFile file)
	{
		_logger.LogInformation($"Загрузка файла {file.FileName}...");

		try
		{
			var fileInfo = file.CreateEntity(Request?.Headers?.Origin!, Request?.Headers?.Authorization!);

			fileInfo!.Id = await _mediator.Send(new UnloadFileCommand(fileInfo, file.OpenReadStream()));

			return Ok(fileInfo.Id);
		}
		catch (Exception ex)
		{
			_logger.LogError("Ошибка при загрузке файла {file}{newLine}{ex}", file, Environment.NewLine, ex);
			throw;
		}
	}

	[HttpGet]
	[Route("download/{id:Guid}")]
	public async Task<IActionResult> Download(Guid id, string title = null!)
	{
		_logger.LogInformation($"Выгрузка файла по id = {id}..");

		var file = await _mediator.Send(new DownloadFileByIdQuery(id));

		if (file is null)
			return NotFound();

		// чтобы не было ошибок в случаи, если файл содержит недопустимые символы, например "-"
		var fileName = WebUtility.UrlEncode(title ?? file.Info.OriginalName);

		// позволяет получать имя файла на клиенте из заголовка, тк иначе не получим, в Angular во всяком случаи
		// не забыть добавить новый заголовок в разрешенные в настройках CORS "ExposedHeaders"
		Response.Headers.Add("x-file-name", $"{fileName}");
		Response.Headers.Add("Access-Control-Expose-Headers", "x-file-name");

		return File(file.Content, file.Info.ContentType, fileName);
	}

	[HttpGet]
	[Route("{id:Guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		_logger.LogInformation($"Получение файла по id = {id}..");

		var file = await _mediator.Send(new DownloadFileByIdQuery(id));

		if (file is null)
			return NotFound();

		return Ok(file.File);
	}
}