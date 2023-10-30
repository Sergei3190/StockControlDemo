using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.Controllers.Select;

[ApiController]
[Authorize]  
[Route("api/v1/select-organizations")]
public class SelectOrganizationsApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<SelectOrganizationsApiController> _logger;

	public SelectOrganizationsApiController(IMediator mediator, ILogger<SelectOrganizationsApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("")]
	public async Task<IActionResult> Select([FromJsonQuery] SelectOrganizationFilterDto filter)
	{
		var result = await _mediator.Send(new GetSelectOrganizationsQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}
}
