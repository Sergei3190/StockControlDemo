using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.Controllers.Select;

[ApiController]
[Authorize]  
[Route("api/v1/select-nomenclatures")]
public class SelectNomenclaturesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<SelectNomenclaturesApiController> _logger;

	public SelectNomenclaturesApiController(IMediator mediator, ILogger<SelectNomenclaturesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("")]
	public async Task<IActionResult> Select([FromJsonQuery] SelectNomenclatureFilterDto filter)
	{
		var result = await _mediator.Send(new GetSelectNomenclaturesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}
}
