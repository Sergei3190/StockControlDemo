using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Models.DTO;

namespace StockControl.API.Controllers.Select;

// партия создаётся, изменяется, удаляется синхронно с документом поступления, отдельный контроллер для этого не делаем
// партия в нашем случаи состоит из номера партии, даты и времени изготовления, если такие есть, номер обязателен
[ApiController]
[Authorize]  
[Route("api/v1/select-parties")]
public class SelectPartiesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<SelectPartiesApiController> _logger;

	public SelectPartiesApiController(IMediator mediator, ILogger<SelectPartiesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("")]
	public async Task<IActionResult> Select([FromJsonQuery] SelectPartyFilterDto filter)
	{
		var result = await _mediator.Send(new GetSelectPartiesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}
}
