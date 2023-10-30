using MediatR;

namespace PersonalCabinet.API.MediatR.Commands.Document;

/// <summary>
/// Команда удаления документа пользователя
/// </summary>
public record DeleteDocumentCommand(Guid Id) : IRequest<bool>
{
}