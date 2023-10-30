namespace Web.StockControl.HttpAggregator.Grpc.Models;

/// <summary>
/// Элемент массива заметок запроса Grpc
/// </summary>
public class NoteArrayItemModel
{
	/// <summary>
	/// Идентификатор заметки
	/// </summary>
	public string Id { get; set; }

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
	public string? ExecutionDate { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(Content)}: {Content} " +
		$" {nameof(IsFix)}: {IsFix}" +
		$" {nameof(Sort)}: {Sort}" +
		$" {nameof(ExecutionDate)}: {ExecutionDate}";
}