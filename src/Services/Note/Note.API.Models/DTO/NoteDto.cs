using Service.Common.DTO.Entities.Base;

namespace Note.API.Models.DTO;

/// <summary>
/// Заметка
/// </summary>
public class NoteDto : EntityDto
{
	/// <summary>
	/// Текст заметки
	/// </summary>
	public string Content { get; set; }

	/// <summary>
	/// Признак избранной заметки
	/// </summary>
	public bool IsFix { get; set; }

	/// <summary>
	/// Номер сортировки
	/// </summary>
	public int Sort { get; set; }

	/// <summary>
	/// Дата выполнения заметки
	/// </summary>
	public DateOnly? ExecutionDate { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(Content)}: {Content} " +
		$" {nameof(IsFix)}: {IsFix}" +
		$" {nameof(Sort)}: {Sort}" +
		$" {nameof(ExecutionDate)}: {ExecutionDate}";
}