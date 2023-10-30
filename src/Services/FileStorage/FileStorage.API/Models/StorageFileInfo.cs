using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileStorage.API.Services;

/// <summary>
/// Информация о файле
/// </summary>
public class StorageFileInfo
{
	public Guid Id { get; set; }

	public string OriginalName { get; set; }

	public string ContentType { get; set; }

	public long ContentLength { get; set; }

	/// <summary>
	/// Инициализация объекта в GridFSBucket MongoDB
	/// </summary>
	[BsonRepresentation(BsonType.ObjectId)]
	public string FileContentId { get; set; }

	/// <summary>
	/// Хост отправитель
	/// </summary>
	public string OriginHost { get; set; }

	/// <summary>
	/// Значение заголовка авторизации (токен доступа)
	/// </summary>
	public string AuthorizationHeader { get; set; }
}