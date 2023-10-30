using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Commands.Nomenclature;
using StockControl.API.MediatR.Commands.Warehouse;
using StockControl.API.MediatR.Queries.Warehouse;
using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.Controllers.ClassifierItems;

[ApiController]
[Authorize]  
[Route("api/v1/warehouses")]
public class WarehousesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<WarehousesApiController> _logger;

	public WarehousesApiController(IMediator mediator, ILogger<WarehousesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetList([FromJsonQuery] WarehouseFilterDto filter)
	{
		var result = await _mediator.Send(new GetWarehousesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await _mediator.Send(new GetWarehouseByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] WarehouseDto dto)
	{
		var id = await _mediator.Send(new CreateWarehouseCommand(dto));

		return CreatedAtAction(nameof(GetById), new { id }, dto);
	}

	[HttpPatch]
	public async Task<IActionResult> Update([FromBody] WarehouseDto dto)
	{
		var result = await _mediator.Send(new UpdateWarehouseCommand(dto));

		if (!result)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await _mediator.Send(new DeleteWarehouseCommand(id));

		if (!result)
			return NotFound();

		return Ok(result);
	}

	// для массовых операций лучше использовать Post или Patch, тк строка запроса ограничена по длине и при большой массовой операции её можеь не хватить,
	// да и а теле запроса данные передавать надёжнее
	[HttpPost("bulk-delete")]
	public async Task<IActionResult> BulkDelete([FromBody] params Guid[] ids)
	{
		var result = await _mediator.Send(new BulkDeleteWarehouseCommand(ids));

		return Ok(result);
	}
}
