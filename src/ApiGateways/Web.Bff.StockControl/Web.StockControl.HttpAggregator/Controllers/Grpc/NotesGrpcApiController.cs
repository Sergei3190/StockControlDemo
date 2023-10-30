using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Web.StockControl.HttpAggregator.Grpc.ClientServices.Intefaces;
using Web.StockControl.HttpAggregator.Grpc.Models;

namespace Web.StockControl.HttpAggregator.Controllers.Grpc;

[ApiController]
[Authorize]
[Route("api/v1/notes-grpc")]
public class NotesGrpcApiController : ControllerBase
{
	private readonly INoteGrpcClientService _clientService;
	private readonly ILogger<NotesGrpcApiController> _logger;

	public NotesGrpcApiController(INoteGrpcClientService clientService, ILogger<NotesGrpcApiController> logger)
	{
		_clientService = clientService;
		_logger = logger;
	}

	[HttpPatch("update-sort")]
	public async Task<IActionResult> UpdateSort([FromBody] NoteArrayItemModel[] dtoArray)
	{
		var result = await _clientService.UpdateSortAsync(dtoArray);

		if (!result)
			return NotFound();

		return Ok(result);
	}
}
