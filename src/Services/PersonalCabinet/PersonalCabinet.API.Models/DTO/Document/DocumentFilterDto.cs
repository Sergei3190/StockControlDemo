using Service.Common.DTO;

namespace PersonalCabinet.API.Models.DTO.Document;

/// <summary>
/// Фильтр загруженных документов персоны
/// </summary>
public class DocumentFilterDto : FilterDto
{
    public Guid? CardId { get; set; }
}