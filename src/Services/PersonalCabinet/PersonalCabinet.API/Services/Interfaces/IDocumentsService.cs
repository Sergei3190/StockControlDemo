using PersonalCabinet.API.Models.DTO.Document;

using Service.Common.Interfaces;

namespace PersonalCabinet.API.Services.Interfaces;

/// <summary>
/// Сервис загруженных документов пользователей
/// </summary>
public interface IDocumentsService : ICrudService<DocumentDto, DocumentFilterDto>
{
}