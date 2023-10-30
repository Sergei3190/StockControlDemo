namespace Service.Common.DTO;

/// <summary>
/// Информация об успешно удаленных элементах операции массового удаления
/// </summary>
public class BulkDeleteSuccessMessageDto
{
    public string? Message { get; set; }
	public IEnumerable<Guid>? Ids { get; set; }
}
