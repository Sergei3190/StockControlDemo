using MediatR;

using PersonalCabinet.API.Models.DTO.Document;

namespace PersonalCabinet.API.MediatR.Queries.Document;

/// <summary>
/// Запрос на получение загруженного пользователем документа по id
/// </summary>
public record GetDocumentByIdQuery(Guid Id) : IRequest<DocumentDto?>
{
}
