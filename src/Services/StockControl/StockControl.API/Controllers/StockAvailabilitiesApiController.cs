using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Queries.StockAvailability;
using StockControl.API.Models.DTO.StockAvailability;

namespace StockControl.API.Controllers;

// Остатки из вне можно только читать.
// удаление, создание и обновление доступно только через создание, удаление и обновление 
// документов движения, таких как поступление, перемещение, списание (в производственном приложение ещё и заявок на отгрузку)

[ApiController]
[Authorize]
[Route("api/v1/stock-availabilities")]
public class StockAvailabilitiesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<StockAvailabilitiesApiController> _logger;

	public StockAvailabilitiesApiController(IMediator mediator, ILogger<StockAvailabilitiesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetList([FromJsonQuery] StockAvailabilityFilterDto filter)
	{
		var result = await _mediator.Send(new GetStockAvailabilitiesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await _mediator.Send(new GetStockAvailabilityByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}
}
