namespace Service.Common.DTO;

/// <summary>
/// Результат массового удаления
/// </summary>
public class BulkDeleteResultDto
{
    public BulkDeleteResultDto()
    {
        ErrorMessage = new List<string>();
    }

    public BulkDeleteSuccessMessageDto? SuccessMessage { get; set; }
	public IList<string>? ErrorMessage { get; set; }
}
