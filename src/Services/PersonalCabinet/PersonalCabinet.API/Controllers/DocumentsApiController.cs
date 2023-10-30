using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PersonalCabinet.API.MediatR.Commands.Document;
using PersonalCabinet.API.MediatR.Queries.Document;
using PersonalCabinet.API.Models.DTO.Document;

using Service.Common.Attributes;

namespace PersonalCabinet.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/person-documents")]
public class DocumentsApiController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<DocumentsApiController> _logger;

	public DocumentsApiController(IMediator mediator, ILogger<DocumentsApiController> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetList([FromJsonQuery] DocumentFilterDto filter)
	{
		var result = await _mediator.Send(new GetDocumentsQuery(filter));

		if (result.TotalItems == 0)
			return NoContent();

		return Ok(result);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await _mediator.Send(new GetDocumentByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] DocumentDto dto)
	{
		var id = await _mediator.Send(new CreateDocumentCommand(dto));

		return CreatedAtAction(nameof(GetById), new { id }, dto);
	}

	[HttpPatch]
	public async Task<IActionResult> Update([FromBody] DocumentDto dto)
	{
		var result = await _mediator.Send(new UpdateDocumentCommand(dto));

		if (!result)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await _mediator.Send(new DeleteDocumentCommand(id));

		if (!result)
			return NotFound();

		return Ok(result);
	}
}
