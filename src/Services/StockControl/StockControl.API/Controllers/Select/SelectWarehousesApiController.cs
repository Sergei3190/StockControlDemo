using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.Controllers.Select;

[ApiController]
[Authorize]  
[Route("api/v1/select-warehouses")]
public class SelectWarehousesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<SelectWarehousesApiController> _logger;

	public SelectWarehousesApiController(IMediator mediator, ILogger<SelectWarehousesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("")]
	public async Task<IActionResult> Select([FromJsonQuery] SelectWarehouseFilterDto filter)
	{
		var result = await _mediator.Send(new GetSelectWarehousesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}
}
