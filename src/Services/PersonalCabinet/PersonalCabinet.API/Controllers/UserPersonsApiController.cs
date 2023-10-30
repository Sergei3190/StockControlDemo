using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PersonalCabinet.API.MediatR.Commands.UserPerson;
using PersonalCabinet.API.MediatR.Queries.UserPerson;
using PersonalCabinet.API.Models.DTO.UserPerson;

using Service.Common.Attributes;

namespace PersonalCabinet.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/user-persons")]
public class UserPersonsApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<UserPersonsApiController> _logger;

	public UserPersonsApiController(IMediator mediator, ILogger<UserPersonsApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetList([FromJsonQuery] UserPersonFilterDto filter)
	{
		var result = await _mediator.Send(new GetUserPersonsQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await _mediator.Send(new GetUserPersonByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpGet("card/{id:Guid}")]
	public async Task<IActionResult> GetByCardId(Guid id)
	{
		var result = await _mediator.Send(new GetUserPersonByCardIdQuery(id));

		if (result is null)
			return NoContent();

		return Ok(result);
	}

	[HttpGet("person-current-user")]
	public async Task<IActionResult> GetPersonCurrentUser()
	{
		var result = await _mediator.Send(new GetUserPersonCurrentUserQuery());

		if (result is null)
			return NoContent();

		return Ok(result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] UserPersonDto dto)
	{
		var id = await _mediator.Send(new CreateUserPersonCommand(dto));

		return CreatedAtAction(nameof(GetById), new { id }, dto);
	}

	[HttpPatch]
	public async Task<IActionResult> Update([FromBody] UserPersonDto dto)
	{
		var result = await _mediator.Send(new UpdateUserPersonCommand(dto));

		if (!result)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await _mediator.Send(new DeleteUserPersonCommand(id));

		if (!result)
			return NotFound();

		return Ok(result);
	}
}
