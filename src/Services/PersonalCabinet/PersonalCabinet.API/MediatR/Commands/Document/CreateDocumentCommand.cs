using MediatR;

using PersonalCabinet.API.Models.DTO.Document;

namespace PersonalCabinet.API.MediatR.Commands.Document;

/// <summary>
/// Команда создания документа пользователя
/// </summary>
public record CreateDocumentCommand(DocumentDto? Dto) : IRequest<Guid>
{
}