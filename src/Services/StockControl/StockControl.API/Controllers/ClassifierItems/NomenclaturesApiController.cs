using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Commands.Nomenclature;
using StockControl.API.MediatR.Queries.Nomenclature;
using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.Controllers.ClassifierItems;

[ApiController]
[Authorize]  
[Route("api/v1/nomenclatures")]
public class NomenclaturesApiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<NomenclaturesApiController> _logger;

    public NomenclaturesApiController(IMediator mediator, ILogger<NomenclaturesApiController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromJsonQuery] NomenclatureFilterDto filter)
    {
        var result = await _mediator.Send(new GetNomenclaturesQuery(filter));

        if (result.TotalItems == 0)
            return NoContent();

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetNomenclatureByIdQuery(id));

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NomenclatureDto dto)
    {
        var id = await _mediator.Send(new CreateNomenclatureCommand(dto));

        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] NomenclatureDto dto)
    {
        var result = await _mediator.Send(new UpdateNomenclatureCommand(dto));

        if (!result)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteNomenclatureCommand(id));

        if (!result)
            return NotFound();

        return Ok(result);
    }

    // для массовых операций лучше использовать Post или Patch, тк строка запроса ограничена по длине и при большой массовой операции её можеь не хватить,
    // да и а теле запроса данные передавать надёжнее
	[HttpPost("bulk-delete")]
	public async Task<IActionResult> BulkDelete([FromBody] params Guid[] ids)
	{
		var result = await _mediator.Send(new BulkDeleteNomenclatureCommand(ids));

		return Ok(result);
	}
}
