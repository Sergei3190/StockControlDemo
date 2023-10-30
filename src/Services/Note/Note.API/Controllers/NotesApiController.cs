using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Note.API.MediatR.Commands;
using Note.API.MediatR.Queries;
using Note.API.Models.DTO;

using Service.Common.Attributes;

namespace Note.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/notes")]
public class NotesApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<NotesApiController> _logger;

	public NotesApiController(IMediator mediator, ILogger<NotesApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetList([FromJsonQuery] NoteFilterDto filter)
	{
		var result = await _mediator.Send(new GetNotesQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await _mediator.Send(new GetNoteByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] NoteDto dto)
	{
		var id = await _mediator.Send(new CreateNoteCommand(dto));

		return CreatedAtAction(nameof(GetById), new { id }, dto);
	}

	[HttpPatch]
	public async Task<IActionResult> Update([FromBody] NoteDto dto)
	{
		var result = await _mediator.Send(new UpdateNoteCommand(dto));

		if (!result)
			return NotFound();

		return Ok(result);
	}

	[HttpPatch("update-sort")]
	public async Task<IActionResult> UpdateSort([FromBody] NoteDto[] dtoArray)
	{
		var result = await _mediator.Send(new UpdateSortCommand(dtoArray));

		if (!result)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await _mediator.Send(new DeleteNoteCommand(id));

		if (!result)
			return NotFound();

		return Ok(result);
	}
}
