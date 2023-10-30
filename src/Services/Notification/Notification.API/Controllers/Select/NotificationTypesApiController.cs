using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Notification.API.MediatR.Queries;
using Notification.API.Models.DTO.NotificationType;

using Service.Common.Attributes;

namespace Notification.API.Controllers.Select;

[ApiController]
[Authorize]
[Route("api/v1/notification-types")]
public class NotificationTypesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<NotificationTypesApiController> _logger;

	public NotificationTypesApiController(IMediator mediator, ILogger<NotificationTypesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet("select")]
	public async Task<IActionResult> Select([FromJsonQuery] NotificationTypeFilterDto filter)
	{
		var result = await _mediator.Send(new GetNotificationTypesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}
}
