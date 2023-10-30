using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PersonalCabinet.API.MediatR.Commands.Photo;
using PersonalCabinet.API.MediatR.Queries.Photo;
using PersonalCabinet.API.Models.DTO.Photo;

using Service.Common.Attributes;

namespace PersonalCabinet.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/person-photos")]
public class PhotosApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<PhotosApiController> _logger;

	public PhotosApiController(IMediator mediator, ILogger<PhotosApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetList([FromJsonQuery] PhotoFilterDto filter)
	{
		var result = await _mediator.Send(new GetPhotosQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await _mediator.Send(new GetPhotoByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpGet("card/{id:Guid}")]
	public async Task<IActionResult> GetByCardId(Guid id)
	{
		var result = await _mediator.Send(new GetPhotoByCardIdQuery(id));

		if (result is null)
			return NoContent();

		return Ok(result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] PhotoDto dto)
	{
		var id = await _mediator.Send(new CreatePhotoCommand(dto));

		return CreatedAtAction(nameof(GetById), new { id }, dto);
	}

	[HttpPatch]
	public async Task<IActionResult> Update([FromBody] PhotoDto dto)
	{
		var result = await _mediator.Send(new UpdatePhotoCommand(dto));

		if (!result)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await _mediator.Send(new DeletePhotoCommand(id));

		if (!result)
			return NotFound();

		return Ok(result);
	}
}
