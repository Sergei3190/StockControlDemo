using MediatR;

using PersonalCabinet.API.Models.DTO.Document;

namespace PersonalCabinet.API.MediatR.Commands.Document;

/// <summary>
/// Команда обновления документа пользователя
/// </summary>
public record UpdateDocumentCommand(DocumentDto? Dto) : IRequest<bool>
{
}