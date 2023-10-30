using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PersonalCabinet.API.MediatR.Queries.Select;
using PersonalCabinet.API.Models.DTO.LoadedDataType;

using Service.Common.Attributes;

namespace PersonalCabinet.API.Controllers.Select;

[ApiController]
[Authorize]
[Route("api/v1/loaded-data-types")]
public class LoadedDataTypesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<LoadedDataTypesApiController> _logger;

	public LoadedDataTypesApiController(IMediator mediator, ILogger<LoadedDataTypesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("select")]
	public async Task<IActionResult> Select([FromJsonQuery] LoadedDataTypeFilterDto filter)
	{
		var result = await _mediator.Send(new GetLoadedDataTypesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}
}
