using MediatR;

using PersonalCabinet.API.Models.DTO.Document;

using Service.Common.DTO;

namespace PersonalCabinet.API.MediatR.Queries.Document;

/// <summary>
/// Запрос на получение отфильтрованных загруженных пользователем документов
/// </summary>
public record GetDocumentsQuery(DocumentFilterDto Filter) : IRequest<PaginatedItemsDto<DocumentDto>>
{
}
