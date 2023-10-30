using Service.Common.DTO;

namespace Note.API.Models.DTO;

/// <summary>
/// Фильтр заметок
/// </summary>
public class NoteFilterDto : FilterDto
{
	public DateOnly? ExecutionDate { get; set; }
}