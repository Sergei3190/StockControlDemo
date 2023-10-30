using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Notification.API.MediatR.Commands;
using Notification.API.MediatR.Queries;
using Notification.API.Models.DTO.NotificationSetting;
using Service.Common.Attributes;

namespace Notification.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/notification-settings")]
public class NotificationSettingsApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<NotificationSettingsApiController> _logger;

	public NotificationSettingsApiController(IMediator mediator, ILogger<NotificationSettingsApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetList([FromJsonQuery] NotificationSettingFilterDto filter)
	{
		var result = await _mediator.Send(new GetNotificationSettingsQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await _mediator.Send(new GetNotificationSettingByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] NotificationSettingDto dto)
	{
		var id = await _mediator.Send(new CreateNotificationSettingCommand(dto));

		return CreatedAtAction(nameof(GetById), new { id }, dto);
	}

	[HttpPatch]
	public async Task<IActionResult> Update([FromBody] NotificationSettingDto dto)
	{
		var result = await _mediator.Send(new UpdateNotificationSettingCommand(dto));

		if (!result)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await _mediator.Send(new DeleteNotificationSettingCommand(id));

		if (!result)
			return NotFound();

		return Ok(result);
	}
}
