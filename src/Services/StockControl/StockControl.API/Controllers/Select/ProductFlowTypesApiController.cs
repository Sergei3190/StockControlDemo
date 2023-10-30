using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Attributes;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Models.DTO.ProductFlowType;

namespace StockControl.API.Controllers.Select;

[ApiController]
[Authorize]  
[Route("api/v1/product-flow-types")]
public class ProductFlowTypesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<ProductFlowTypesApiController> _logger;

	public ProductFlowTypesApiController(IMediator mediator, ILogger<ProductFlowTypesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("select")]
	public async Task<IActionResult> Select([FromJsonQuery] ProductFlowTypeFilterDto filter)
	{
		var result = await _mediator.Send(new GetProductFlowTypesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}
}
