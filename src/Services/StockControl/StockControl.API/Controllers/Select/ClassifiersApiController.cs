using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Models.DTO.Classifier;

namespace StockControl.API.Controllers.Select;

[ApiController]
[Authorize]  
[Route("api/v1/classifiers")]
public class ClassifiersApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<ClassifiersApiController> _logger;

	public ClassifiersApiController(IMediator mediator, ILogger<ClassifiersApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("select")]
	public async Task<IActionResult> Select([FromJsonQuery] ClassifierFilterDto filter)
	{
		var result = await _mediator.Send(new GetClassifiersQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}
}
